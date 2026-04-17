using Microsoft.Data.SqlClient;

namespace APIMASTER.Services;

public interface IDatabaseResolver
{
    SqlConnection GetConnection(string module);
    SqlConnection GetConnectionByName(string connectionStringName);
}
