using APIMASTER.Models.Requests;
using FluentValidation;

namespace APIMASTER.Validation;

public class MilkDairiesRequestValidator : AbstractValidator<MilkDairiesRequest>
{
    public MilkDairiesRequestValidator()
    {
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty().GreaterThan(x => x.StartDate);
        RuleFor(x => x.LocationId).GreaterThan(0);
    }
}
