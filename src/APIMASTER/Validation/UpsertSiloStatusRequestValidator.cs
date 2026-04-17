using APIMASTER.Models.Requests;
using FluentValidation;

namespace APIMASTER.Validation;

public class UpsertSiloStatusRequestValidator : AbstractValidator<UpsertSiloStatusRequest>
{
    public UpsertSiloStatusRequestValidator()
    {
        RuleFor(x => x.IdSilo).GreaterThan(0);
        RuleFor(x => x.IdUser).GreaterThan(0);
        RuleFor(x => x.CurrentWeight).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Capacity).GreaterThan(0);

        RuleFor(x => x.Images)
            .Must(imgs => imgs.Count <= 20)
            .WithMessage("Maximum 20 images per request.");

        RuleForEach(x => x.Images)
            .Must(f => f.Length <= 10 * 1024 * 1024)
            .WithMessage("Each image must not exceed 10 MB.");
    }
}
