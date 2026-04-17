using APIMASTER.Models.Requests;
using APIMASTER.Validation;
using FluentValidation.TestHelper;

namespace APIMASTER.Tests.Validation;

public class ChangeStatusRequestValidatorTests
{
    private readonly ChangeStatusRequestValidator _validator = new();

    [Theory]
    [InlineData("approved")]
    [InlineData("rejected")]
    [InlineData("pending")]
    public void Should_Pass_With_Valid_Status(string status)
    {
        var request = new ChangeStatusRequest
        {
            IdQuestion = 1,
            IdUser = 1,
            Status = status
        };

        var result = _validator.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Fail_With_Invalid_Status()
    {
        var request = new ChangeStatusRequest
        {
            IdQuestion = 1,
            IdUser = 1,
            Status = "cancelled"
        };

        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Status);
    }

    [Fact]
    public void Should_Fail_When_IdQuestion_Is_Zero()
    {
        var request = new ChangeStatusRequest
        {
            IdQuestion = 0,
            IdUser = 1,
            Status = "approved"
        };

        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.IdQuestion);
    }
}
