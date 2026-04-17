using APIMASTER.Data.Entities;

namespace APIMASTER.Services;

public interface ICrossCatalogService
{
    Task<IEnumerable<CrossCustomerDairyBarnSilo>> GetCustomerDairyBarnSilosAsync();
    Task<IEnumerable<CrossTruck>> GetTrucksAsync();
    Task<IEnumerable<CrossDriver>> GetDriversAsync();
    Task<IEnumerable<CrossCatalog>> GetCatalogsByTypeAsync(string type);
}
