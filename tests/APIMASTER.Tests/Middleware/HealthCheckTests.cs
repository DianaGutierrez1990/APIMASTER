namespace APIMASTER.Tests.Middleware;

public class HealthCheckTests
{
    [Fact]
    public void Health_Endpoint_Route_Is_Correct()
    {
        // Verify the health controller is configured at the expected route
        var controllerType = typeof(APIMASTER.Controllers.V1.HealthController);
        var routeAttr = controllerType.GetCustomAttributes(typeof(Microsoft.AspNetCore.Mvc.RouteAttribute), false);
        Assert.Single(routeAttr);

        var route = (Microsoft.AspNetCore.Mvc.RouteAttribute)routeAttr[0];
        Assert.Equal("health", route.Template);
    }

    [Fact]
    public void Health_Endpoint_Allows_Anonymous()
    {
        var controllerType = typeof(APIMASTER.Controllers.V1.HealthController);
        var anonymousAttr = controllerType.GetCustomAttributes(typeof(Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute), false);
        Assert.Single(anonymousAttr);
    }
}
