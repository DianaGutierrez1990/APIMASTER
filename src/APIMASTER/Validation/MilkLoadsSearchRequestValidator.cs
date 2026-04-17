using APIMASTER.Models.Requests;
using FluentValidation;

namespace APIMASTER.Validation;

public class MilkLoadsSearchRequestValidator : AbstractValidator<MilkLoadsSearchRequest>
{
    public MilkLoadsSearchRequestValidator()
    {
        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.Ticket) || !string.IsNullOrWhiteSpace(x.Manifest))
            .WithMessage("At least one of Ticket or Manifest must be provided.");
    }
}
