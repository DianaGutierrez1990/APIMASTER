using APIMASTER.Models.Requests;
using FluentValidation;

namespace APIMASTER.Validation;

public class CommodityLoadsRequestValidator : AbstractValidator<CommodityLoadsRequest>
{
    public CommodityLoadsRequestValidator()
    {
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty().GreaterThan(x => x.StartDate);
        RuleFor(x => x.CommodityId).GreaterThan(0);
    }
}
