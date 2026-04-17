using APIMASTER.Models.Requests;
using FluentValidation;

namespace APIMASTER.Validation;

public class CrossUserLoadsRequestValidator : AbstractValidator<CrossUserLoadsRequest>
{
    public CrossUserLoadsRequestValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.DaysBefore).InclusiveBetween(1, 365);
    }
}
