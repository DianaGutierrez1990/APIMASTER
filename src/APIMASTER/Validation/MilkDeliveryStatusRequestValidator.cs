using APIMASTER.Models.Requests;
using FluentValidation;

namespace APIMASTER.Validation;

public class MilkDeliveryStatusRequestValidator : AbstractValidator<MilkDeliveryStatusRequest>
{
    public MilkDeliveryStatusRequestValidator()
    {
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty().GreaterThan(x => x.StartDate);
        RuleFor(x => x.CustomerId).GreaterThan(0);
    }
}
