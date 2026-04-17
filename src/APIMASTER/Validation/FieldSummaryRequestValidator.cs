using APIMASTER.Models.Requests;
using FluentValidation;

namespace APIMASTER.Validation;

public class FieldSummaryRequestValidator : AbstractValidator<FieldSummaryRequest>
{
    public FieldSummaryRequestValidator()
    {
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty().GreaterThan(x => x.StartDate);
    }
}
