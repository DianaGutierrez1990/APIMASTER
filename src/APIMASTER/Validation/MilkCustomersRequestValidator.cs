using APIMASTER.Models.Requests;
using FluentValidation;

namespace APIMASTER.Validation;

public class MilkCustomersRequestValidator : AbstractValidator<MilkCustomersRequest>
{
    public MilkCustomersRequestValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("StartDate is required.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("EndDate is required.")
            .GreaterThan(x => x.StartDate).WithMessage("EndDate must be after StartDate.");

        RuleFor(x => x.LocationId)
            .GreaterThan(0).WithMessage("LocationId must be greater than 0.");
    }
}
