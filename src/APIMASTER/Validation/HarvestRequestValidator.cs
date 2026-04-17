using APIMASTER.Models.Requests;
using FluentValidation;

namespace APIMASTER.Validation;

public class HarvestRequestValidator : AbstractValidator<HarvestRequest>
{
    public HarvestRequestValidator()
    {
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty().GreaterThan(x => x.StartDate);
    }
}
