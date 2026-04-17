-- ═══════════════════════════════════════════════════════════════
-- Verify RLS is working correctly
-- ═══════════════════════════════════════════════════════════════

-- Test 1: Set a tenant context and verify filtering
EXEC sp_set_session_context @key = N'TenantId', @value = '00000000-0000-0000-0000-000000000001';

-- Should only return rows for tenant 00000000-0000-0000-0000-000000000001
-- SELECT * FROM dbo.YourTable;

-- Test 2: Verify SESSION_CONTEXT is set
SELECT CAST(SESSION_CONTEXT(N'TenantId') AS UNIQUEIDENTIFIER) AS CurrentTenantId;

-- Test 3: List all security policies
SELECT
    sp.name AS PolicyName,
    sp.is_enabled,
    o.name AS TableName,
    pred.predicate_definition
FROM sys.security_policies sp
JOIN sys.security_predicates pred ON sp.object_id = pred.object_id
JOIN sys.objects o ON pred.target_object_id = o.object_id
ORDER BY sp.name, o.name;

-- Test 4: Try to insert a row with wrong tenant (should be blocked)
-- EXEC sp_set_session_context @key = N'TenantId', @value = '00000000-0000-0000-0000-000000000001';
-- INSERT INTO dbo.YourTable (TenantId, ...) VALUES ('99999999-9999-9999-9999-999999999999', ...);
-- Expected: error blocked by BLOCK predicate
