# CLAUDE.md — Instrucciones del proyecto

## Qué es este proyecto
API .NET 8 multi-tenant que reemplaza una API Python (Flask). Conecta a 6 bases de datos SQL Server. Las funciones SQL (TVFs y Stored Procedures) ya existen y NO se modifican — esta API solo las llama.

## Contexto completo
Lee el archivo `CONTEXTO_PROYECTO_API.md` en la raíz del proyecto. Ahí está TODO: la estructura de la API Python original, los 40+ endpoints a migrar, las funciones SQL Server, la arquitectura objetivo, las decisiones de diseño, y el plan fase por fase.

## Reglas del proyecto

### Stack
- .NET 8 Web API
- Dapper para llamar TVFs y Stored Procedures de SQL Server (NO EF Core para queries existentes)
- EF Core solo para el multi-tenancy (global query filters, interceptor SESSION_CONTEXT)
- FluentValidation para validación de input
- JWT Bearer Authentication (Keycloak como IdP)
- Serilog para logging

### Estructura de carpetas
Seguir la estructura definida en CONTEXTO_PROYECTO_API.md sección 3.

### Patrón de cada endpoint
1. Request DTO en Models/Requests/ con FluentValidation
2. Service con interfaz en Services/ que llama la TVF o SP via Dapper con parámetros (@param, NUNCA concatenar strings)
3. Controller en Controllers/V1/{Modulo}/ con el verbo HTTP correcto
4. GETs para lectura, POSTs para crear, PUTs para actualizar, DELETEs para borrar

### Seguridad (obligatorio)
- NUNCA concatenar valores en SQL — siempre parámetros
- NUNCA hardcodear credenciales — todo en appsettings.json
- Todos los endpoints llevan [Authorize]
- Validar todo input con FluentValidation
- Retornar códigos HTTP correctos (no 200 para errores)

### Conexiones a BD
Hay 6 bases de datos. Cada módulo usa una BD específica:
- dairy mobile → GeneralDairyi
- cross → Cross
- scalei → ICCManager y GeneralDairyi
- approvals → ApprovalProd
- general → General

### Cómo ejecutar
```bash
dotnet restore
dotnet build
dotnet run
# Swagger en https://localhost:5001/swagger
```

### Cómo probar
```bash
dotnet test
```
