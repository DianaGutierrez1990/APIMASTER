-- ═══════════════════════════════════════════════════════════════
-- Row-Level Security (RLS) Setup for Multi-Tenant Isolation
-- Run this script on EACH database that needs tenant isolation.
--
-- Prerequisites:
-- 1. Each table that needs isolation must have a TenantId column (UNIQUEIDENTIFIER)
-- 2. The API sets SESSION_CONTEXT('TenantId') on every connection via TenantDbInterceptor
-- ═══════════════════════════════════════════════════════════════

-- Step 1: Create the tenant filter function
IF OBJECT_ID('dbo.fn_TenantFilter', 'IF') IS NOT NULL
    DROP FUNCTION dbo.fn_TenantFilter;
GO

CREATE FUNCTION dbo.fn_TenantFilter(@TenantId UNIQUEIDENTIFIER)
RETURNS TABLE
WITH SCHEMABINDING
AS
RETURN
    SELECT 1 AS result
    WHERE @TenantId = CAST(SESSION_CONTEXT(N'TenantId') AS UNIQUEIDENTIFIER)
       OR SESSION_CONTEXT(N'TenantId') IS NULL; -- Allow access when no tenant context (admin/migrations)
GO

-- Step 2: Create security policy
-- IMPORTANT: Add each table that needs tenant isolation below.
-- Example for a "Products" table:
--
-- IF EXISTS (SELECT 1 FROM sys.security_policies WHERE name = 'TenantSecurityPolicy')
--     DROP SECURITY POLICY dbo.TenantSecurityPolicy;
-- GO
--
-- CREATE SECURITY POLICY dbo.TenantSecurityPolicy
--     ADD FILTER PREDICATE dbo.fn_TenantFilter(TenantId) ON dbo.Products,
--     ADD BLOCK  PREDICATE dbo.fn_TenantFilter(TenantId) ON dbo.Products
--     WITH (STATE = ON, SCHEMABINDING = OFF);
-- GO

-- ═══════════════════════════════════════════════════════════════
-- TEMPLATE: Copy and adapt for each table
-- ═══════════════════════════════════════════════════════════════
--
-- Step A: Add TenantId column if it doesn't exist
-- ALTER TABLE dbo.YourTable ADD TenantId UNIQUEIDENTIFIER NULL;
-- GO
--
-- Step B: Backfill existing rows with a default tenant
-- UPDATE dbo.YourTable SET TenantId = '00000000-0000-0000-0000-000000000001' WHERE TenantId IS NULL;
-- GO
--
-- Step C: Make it NOT NULL after backfill
-- ALTER TABLE dbo.YourTable ALTER COLUMN TenantId UNIQUEIDENTIFIER NOT NULL;
-- GO
--
-- Step D: Add index for performance
-- CREATE NONCLUSTERED INDEX IX_YourTable_TenantId ON dbo.YourTable(TenantId);
-- GO
--
-- Step E: Add to the security policy (see Step 2 above)
-- ═══════════════════════════════════════════════════════════════

PRINT 'RLS setup complete. Add tables to the security policy as needed.';
GO
