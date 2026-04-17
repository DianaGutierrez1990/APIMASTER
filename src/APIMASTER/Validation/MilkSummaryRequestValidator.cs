using APIMASTER.Models.Requests;
using FluentValidation;

namespace APIMASTER.Validation;

public class MilkSummaryRequestValidator : AbstractValidator<MilkSummaryRequest>
{
    private static readonly string[] ValidGroups = ["Week", "Month", "Quarter", "Year"];

    public MilkSummaryRequestValidator()
    {
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty().GreaterThan(x => x.StartDate);
        RuleFor(x => x.Group)
            .NotEmpty()
            .Must(g => ValidGroups.Contains(g))
            .WithMessage($"Group must be one of: {string.Join(", ", ValidGroups)}");
    }
}
