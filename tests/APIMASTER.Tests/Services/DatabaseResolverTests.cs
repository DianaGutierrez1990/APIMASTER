using APIMASTER.Services;
using Microsoft.Extensions.Configuration;

namespace APIMASTER.Tests.Services;

public class DatabaseResolverTests
{
    private readonly DatabaseResolver _resolver;

    public DatabaseResolverTests()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:GeneralDairyi"] = "Server=test;Database=Test;",
                ["ConnectionStrings:Cross"] = "Server=test;Database=Cross;",
                ["ConnectionStrings:ICCManager"] = "Server=test;Database=ICCManager;",
                ["ConnectionStrings:ApprovalProd"] = "Server=test;Database=ApprovalProd;",
                ["ConnectionStrings:General"] = "Server=test;Database=General;",
                ["ConnectionStrings:iScaleOutlier"] = "Server=test;Database=iScaleOutlier;",
            })
            .Build();

        _resolver = new DatabaseResolver(config);
    }

    [Theory]
    [InlineData("dairy", "Server=test;Database=Test;")]
    [InlineData("milk", "Server=test;Database=Test;")]
    [InlineData("harvest", "Server=test;Database=Test;")]
    [InlineData("cross", "Server=test;Database=Cross;")]
    [InlineData("captures", "Server=test;Database=Cross;")]
    [InlineData("scalei", "Server=test;Database=ICCManager;")]
    [InlineData("approvals", "Server=test;Database=ApprovalProd;")]
    public void GetConnection_Returns_Correct_Connection_For_Module(string module, string expectedConnString)
    {
        using var conn = _resolver.GetConnection(module);
        Assert.Equal(expectedConnString, conn.ConnectionString);
    }

    [Fact]
    public void GetConnection_Throws_For_Unknown_Module()
    {
        Assert.Throws<ArgumentException>(() => _resolver.GetConnection("unknown"));
    }

    [Fact]
    public void GetConnectionByName_Throws_For_Missing_ConnectionString()
    {
        Assert.Throws<InvalidOperationException>(() => _resolver.GetConnectionByName("NonExistent"));
    }

    [Fact]
    public void GetConnection_Is_Case_Insensitive()
    {
        using var conn1 = _resolver.GetConnection("DAIRY");
        using var conn2 = _resolver.GetConnection("dairy");
        Assert.Equal(conn1.ConnectionString, conn2.ConnectionString);
    }
}
