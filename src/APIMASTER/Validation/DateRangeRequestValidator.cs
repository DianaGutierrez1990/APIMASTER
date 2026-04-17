using APIMASTER.Models.Requests;
using FluentValidation;

namespace APIMASTER.Validation;

public class DateRangeRequestValidator : AbstractValidator<DateRangeRequest>
{
    public DateRangeRequestValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("StartDate is required.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("EndDate is required.")
            .GreaterThan(x => x.StartDate).WithMessage("EndDate must be after StartDate.");
    }
}
