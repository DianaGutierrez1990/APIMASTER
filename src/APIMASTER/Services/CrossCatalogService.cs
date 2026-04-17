using Dapper;
using APIMASTER.Data.Entities;

namespace APIMASTER.Services;

public class CrossCatalogService : ICrossCatalogService
{
    private readonly IDatabaseResolver _dbResolver;

    public CrossCatalogService(IDatabaseResolver dbResolver)
    {
        _dbResolver = dbResolver;
    }

    public async Task<IEnumerable<CrossCustomerDairyBarnSilo>> GetCustomerDairyBarnSilosAsync()
    {
        const string sql = "SELECT * FROM MV_List_Customer_Dairy_Barn_Silo() ORDER BY Customer_Name ASC";

        using var conn = _dbResolver.GetConnection("cross");
        await conn.OpenAsync();
        return await conn.QueryAsync<CrossCustomerDairyBarnSilo>(sql);
    }

    public async Task<IEnumerable<CrossTruck>> GetTrucksAsync()
    {
        const string sql = "SELECT * FROM MV_List_Trucks() WHERE Active = 1 ORDER BY Truck_Name ASC";

        using var conn = _dbResolver.GetConnection("cross");
        await conn.OpenAsync();
        return await conn.QueryAsync<CrossTruck>(sql);
    }

    public async Task<IEnumerable<CrossDriver>> GetDriversAsync()
    {
        const string sql = "SELECT * FROM MV_List_Drivers() WHERE Active = 1 ORDER BY Driver_Name ASC";

        using var conn = _dbResolver.GetConnection("cross");
        await conn.OpenAsync();
        return await conn.QueryAsync<CrossDriver>(sql);
    }

    public async Task<IEnumerable<CrossCatalog>> GetCatalogsByTypeAsync(string type)
    {
        const string sql = "SELECT * FROM MV_List_Catalogs(@Type) WHERE Active = 1 ORDER BY Name ASC";

        using var conn = _dbResolver.GetConnection("cross");
        await conn.OpenAsync();
        return await conn.QueryAsync<CrossCatalog>(sql, new { Type = type });
    }
}
