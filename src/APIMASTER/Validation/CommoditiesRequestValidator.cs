using APIMASTER.Models.Requests;
using FluentValidation;

namespace APIMASTER.Validation;

public class CommoditiesRequestValidator : AbstractValidator<CommoditiesRequest>
{
    public CommoditiesRequestValidator()
    {
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty().GreaterThan(x => x.StartDate);
    }
}
