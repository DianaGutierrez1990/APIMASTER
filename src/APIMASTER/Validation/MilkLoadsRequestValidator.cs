using APIMASTER.Models.Requests;
using FluentValidation;

namespace APIMASTER.Validation;

public class MilkLoadsRequestValidator : AbstractValidator<MilkLoadsRequest>
{
    public MilkLoadsRequestValidator()
    {
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty().GreaterThan(x => x.StartDate);
        RuleFor(x => x.DairyId).GreaterThan(0);
        RuleFor(x => x.CustomerLocationId).GreaterThan(0);
    }
}
