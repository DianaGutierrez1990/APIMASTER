using Microsoft.Data.SqlClient;

namespace APIMASTER.Services;

public class DatabaseResolver : IDatabaseResolver
{
    private readonly IConfiguration _configuration;

    private static readonly Dictionary<string, string> ModuleToConnectionMap = new(StringComparer.OrdinalIgnoreCase)
    {
        ["dairy"] = "ICCManager",
        ["milk"] = "ICCManager",
        ["harvest"] = "ICCManager",
        ["commodities"] = "ICCManager",
        ["transfer"] = "ICCManager",
        ["cross"] = "Cross",
        ["captures"] = "Cross",
        ["catalogs"] = "Cross",
        ["scalei"] = "ICCManager",
        ["approvals"] = "ApprovalProd",
        ["general"] = "General",
        ["outlier"] = "iScaleOutlier",
    };

    public DatabaseResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public SqlConnection GetConnection(string module)
    {
        if (!ModuleToConnectionMap.TryGetValue(module, out var connectionName))
        {
            throw new ArgumentException($"Unknown module: '{module}'. Valid modules: {string.Join(", ", ModuleToConnectionMap.Keys)}");
        }

        return GetConnectionByName(connectionName);
    }

    public SqlConnection GetConnectionByName(string connectionStringName)
    {
        var connectionString = _configuration.GetConnectionString(connectionStringName)
            ?? throw new InvalidOperationException($"Connection string '{connectionStringName}' not found in configuration.");

        return new SqlConnection(connectionString);
    }
}
