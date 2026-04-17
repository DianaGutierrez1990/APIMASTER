using APIMASTER.Models.Requests;
using FluentValidation;

namespace APIMASTER.Validation;

public class MilkImageRequestValidator : AbstractValidator<MilkImageRequest>
{
    public MilkImageRequestValidator()
    {
        RuleFor(x => x.MilkLoadId).GreaterThan(0);
    }
}
