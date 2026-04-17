# Flujo de trabajo del API Multi-Tenant

**Propuesta de arquitectura alineada al modelo de BD existente (ID_Business)**
Autor: Diana Gutiérrez — Intelogix
Fecha: 2026-04-16

---

## 1. Modelo actual de la empresa (lo que ya existe)

Cada software/producto (Desk) maneja múltiples empresas. Todas las empresas viven en una misma BD, diferenciadas por una columna `ID_Business` (uniqueidentifier).

```
Servidor SQL (35.223.136.179)
│
├── CreditCharges              → Tarjetas, transacciones, gastos
├── Diesel / Diesel_v3         → Combustible
├── Alimentos_Integrados       → Alimentación
├── InventoryAsset             → Vehículos / activos
├── Payroll                    → Nómina
├── StaffManagerUtilities      → Contratos, vacaciones, warnings
├── Employees_General_Info     → Catálogo maestro de empleados
└── Approval_Prod              → Aprobaciones
```

**Catálogo central de empresas**: `CreditCharges.web.vw_business`
→ `id_business (GUID)`, `short_name`, `business`

Ejemplos: Intepizzaco, Airman Property Development, Scoria, Veribest, Lipan Cattle Feeder, In4Life Farms, Richies Towing, etc.

Cada tabla operativa (empleados, transacciones, vehículos, combustible, alimentos) lleva `ID_Business` para saber a qué empresa pertenece el registro.

---

## 2. Cómo el API replica ese modelo

El API .NET 10 NO cambia la BD. Solo expone endpoints HTTP que:

1. Reciben un **token JWT** del usuario (emitido por Keycloak).
2. Del token extraen el **`ID_Business`** del claim.
3. Inyectan automáticamente `@ID_Business` como parámetro en cada query / TVF / SP.
4. Devuelven únicamente los datos de esa empresa.

### Flujo de un request

```
┌─────────────┐    1. Login       ┌──────────┐
│  Frontend   │──────────────────>│ Keycloak │
│ (StaffMgr)  │<── JWT con claims─│   IdP    │
└─────────────┘                   └──────────┘
       │
       │  2. GET /api/v1/staff/employees
       │     Authorization: Bearer <JWT>
       ▼
┌────────────────────────────────────────────┐
│            API .NET 10                     │
│                                            │
│  [Authorize] → extrae ID_Business del JWT  │
│       │                                    │
│       ▼                                    │
│  Service (Dapper)                          │
│       │ EXEC TVF_List_Employees            │
│       │      @ID_Business = '...'          │
│       ▼                                    │
│  SQL Server (CreditCharges / HR / etc.)    │
│       │                                    │
│       ▼                                    │
│  Filtra WHERE ID_Business = @ID_Business   │
└────────────────────────────────────────────┘
       │
       ▼  JSON con solo los datos de esa empresa
┌─────────────┐
│  Frontend   │
└─────────────┘
```

---

## 3. Por qué este modelo es seguro

| Problema de la API Python actual | Cómo lo resuelve la API .NET |
|----------------------------------|------------------------------|
| Credenciales hardcodeadas en código | `appsettings.json` + Secret Manager / Env vars |
| SQL concatenado → SQL Injection | Parámetros Dapper (`@ID_Business`) — imposible inyectar |
| Sin autenticación | JWT Bearer obligatorio en todos los endpoints (`[Authorize]`) |
| Sin filtro de empresa → cualquier usuario ve todo | `ID_Business` del JWT se inyecta en toda query |
| Sin validación de input | FluentValidation en cada request DTO |
| Sin logging de auditoría | Serilog registra usuario + ID_Business + endpoint |

---

## 4. Estructura del API por módulo

Cada módulo del Desk (StaffManager, CreditCharges, Diesel, Alimentos, Vehículos) tiene su propio conjunto de endpoints, pero TODOS comparten el mismo patrón de filtrado por `ID_Business`.

```
/api/v1/
  ├── staff/        → Empleados, contratos, vacaciones, warnings
  │    ├── GET    /employees
  │    ├── POST   /employees
  │    ├── PUT    /employees/{id}
  │    ├── DELETE /employees/{id}
  │    ├── GET    /contracts
  │    ├── GET    /vacations
  │    └── GET    /warnings
  │
  ├── credit-charges/  → Tarjetas, transacciones
  │    ├── GET    /credit-cards
  │    ├── GET    /transactions
  │    └── POST   /transactions
  │
  ├── diesel/       → Combustible
  │    ├── GET    /fuel-loads
  │    └── POST   /fuel-loads
  │
  ├── food/         → Alimentos
  │    └── GET    /consumption
  │
  ├── vehicles/     → Vehículos
  │    ├── GET    /vehicles
  │    └── POST   /vehicles
  │
  └── businesses/   → Catálogo de empresas (solo lectura)
       └── GET    /businesses   (las que el usuario puede ver)
```

---

## 5. Flujo de desarrollo por módulo

Para cada módulo se ejecutan los mismos 5 pasos:

1. **Introspección de BD**
   Revisar columnas reales de las TVFs/SPs y tablas (ya sabemos que el discriminador es `ID_Business`).

2. **Entidades C#**
   Crear clases que reflejan 1:1 las columnas reales (sin inventar nombres).

3. **Request DTOs + FluentValidation**
   Validar input antes de llegar a la BD.

4. **Service con Dapper**
   Llamar TVF/SP con parámetros. `ID_Business` se obtiene del JWT, NO del body del request.

5. **Controller con `[Authorize]`**
   Ruta REST, verbos HTTP correctos (GET/POST/PUT/DELETE), respuestas con código HTTP adecuado.

---

## 6. Plan de entrega por fases

| Fase | Módulo | Duración estimada | Entregable |
|------|--------|-------------------|------------|
| 1 | Infraestructura multi-tenant (claim `ID_Business` del JWT, middleware de inyección, validación de acceso) | ✅ En curso | Base reutilizable para todos los módulos |
| 2 | **Staff Manager** (empleados, contratos, vacaciones, warnings) | 1 sprint | Endpoints del StaffManager listos |
| 3 | **CreditCharges** (tarjetas + transacciones) | 1 sprint | Frontend CreditCharges conectado al nuevo API |
| 4 | **Diesel** | 0.5 sprint | Endpoints de combustible |
| 5 | **Alimentos** | 0.5 sprint | Endpoints de alimentación |
| 6 | **Vehículos** (InventoryAsset) | 0.5 sprint | Endpoints de vehículos |
| 7 | **Approvals** | 0.5 sprint | Endpoints de aprobaciones |
| 8 | Cutover — apagar API Python, toda la plataforma consume el API .NET | — | Plataforma unificada |

**Ya completado (previo a este flujo)**:
- Scalei (8 endpoints productivos)
- DairyMobile (entidades verificadas)
- Migración a .NET 10
- Autenticación JWT + DevAuthHandler
- Dropbox reemplazando FTP

---

## 7. Beneficios para el negocio

- **Una sola API** para todos los productos del Desk (StaffManager, CreditCharges, Diesel, etc.) — el frontend llama a un único `api.intelogix.mx`.
- **Seguridad de nivel empresa**: un usuario de la empresa A nunca puede ver datos de la empresa B, aunque manipule el frontend — el API filtra en servidor por el JWT.
- **Escalable**: agregar una empresa nueva = insertar un row en `web.vw_business`, sin tocar código.
- **Auditable**: cada request queda registrado con usuario + ID_Business + acción.
- **Migración gradual**: los módulos se van moviendo de Python a .NET uno por uno, sin romper nada.

---

## 8. Qué necesito confirmar antes de arrancar Fase 2

1. ¿El JWT de Keycloak ya incluye el claim `id_business`, o hay que agregarlo en la configuración del realm?
2. ¿Un usuario puede pertenecer a más de una empresa? Si sí, ¿el JWT trae una lista de `ID_Business` y el frontend elige cuál usar por request (header `X-Business-Id`)?
3. ¿Empezamos Fase 2 por **Empleados** (StaffManager) o hay otro módulo más urgente?

---

*Fin del documento.*
