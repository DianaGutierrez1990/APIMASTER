using APIMASTER.Models.Requests;

namespace APIMASTER.Tests.Models;

public class PaginationParamsTests
{
    [Fact]
    public void Default_Values_Are_Correct()
    {
        var pagination = new PaginationParams();
        Assert.Equal(1, pagination.Page);
        Assert.Equal(20, pagination.PageSize);
        Assert.Equal(0, pagination.Skip);
    }

    [Fact]
    public void PageSize_Is_Clamped_To_Max()
    {
        var pagination = new PaginationParams { PageSize = 500 };
        Assert.Equal(100, pagination.PageSize);
    }

    [Fact]
    public void Page_Cannot_Be_Less_Than_One()
    {
        var pagination = new PaginationParams { Page = 0 };
        Assert.Equal(1, pagination.Page);

        pagination.Page = -5;
        Assert.Equal(1, pagination.Page);
    }

    [Fact]
    public void Skip_Is_Calculated_Correctly()
    {
        var pagination = new PaginationParams { Page = 3, PageSize = 10 };
        Assert.Equal(20, pagination.Skip);
    }

    [Fact]
    public void PageSize_Defaults_When_Zero()
    {
        var pagination = new PaginationParams { PageSize = 0 };
        Assert.Equal(20, pagination.PageSize);
    }
}
