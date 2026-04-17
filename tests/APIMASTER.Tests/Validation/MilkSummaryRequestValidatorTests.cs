using APIMASTER.Models.Requests;
using APIMASTER.Validation;
using FluentValidation.TestHelper;

namespace APIMASTER.Tests.Validation;

public class MilkSummaryRequestValidatorTests
{
    private readonly MilkSummaryRequestValidator _validator = new();

    [Theory]
    [InlineData("Week")]
    [InlineData("Month")]
    [InlineData("Quarter")]
    [InlineData("Year")]
    public void Should_Pass_With_Valid_Group(string group)
    {
        var request = new MilkSummaryRequest
        {
            StartDate = DateTime.Today.AddDays(-30),
            EndDate = DateTime.Today,
            Group = group
        };

        var result = _validator.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(x => x.Group);
    }

    [Theory]
    [InlineData("Daily")]
    [InlineData("Yearly")]
    [InlineData("")]
    [InlineData("invalid")]
    public void Should_Fail_With_Invalid_Group(string group)
    {
        var request = new MilkSummaryRequest
        {
            StartDate = DateTime.Today.AddDays(-30),
            EndDate = DateTime.Today,
            Group = group
        };

        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Group);
    }
}
