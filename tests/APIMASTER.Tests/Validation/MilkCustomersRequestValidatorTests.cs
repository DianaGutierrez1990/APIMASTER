using APIMASTER.Models.Requests;
using APIMASTER.Validation;
using FluentValidation.TestHelper;

namespace APIMASTER.Tests.Validation;

public class MilkCustomersRequestValidatorTests
{
    private readonly MilkCustomersRequestValidator _validator = new();

    [Fact]
    public void Should_Pass_With_Valid_Request()
    {
        var request = new MilkCustomersRequest
        {
            StartDate = DateTime.Today.AddDays(-30),
            EndDate = DateTime.Today,
            LocationId = 1
        };

        var result = _validator.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Fail_When_StartDate_Is_Empty()
    {
        var request = new MilkCustomersRequest
        {
            StartDate = default,
            EndDate = DateTime.Today,
            LocationId = 1
        };

        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.StartDate);
    }

    [Fact]
    public void Should_Fail_When_EndDate_Before_StartDate()
    {
        var request = new MilkCustomersRequest
        {
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(-1),
            LocationId = 1
        };

        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.EndDate);
    }

    [Fact]
    public void Should_Fail_When_LocationId_Is_Zero()
    {
        var request = new MilkCustomersRequest
        {
            StartDate = DateTime.Today.AddDays(-30),
            EndDate = DateTime.Today,
            LocationId = 0
        };

        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.LocationId);
    }

    [Fact]
    public void Should_Fail_When_LocationId_Is_Negative()
    {
        var request = new MilkCustomersRequest
        {
            StartDate = DateTime.Today.AddDays(-30),
            EndDate = DateTime.Today,
            LocationId = -5
        };

        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.LocationId);
    }
}
