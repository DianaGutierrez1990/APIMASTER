using APIMASTER.Models.Requests;
using FluentValidation;

namespace APIMASTER.Validation;

public class UpsertMilkLoadWithImagesRequestValidator : AbstractValidator<UpsertMilkLoadWithImagesRequest>
{
    public UpsertMilkLoadWithImagesRequestValidator()
    {
        RuleFor(x => x.IdCustomerDairyBarnSilo).GreaterThan(0);
        RuleFor(x => x.IdUser).GreaterThan(0);
        RuleFor(x => x.GrossWeight).GreaterThanOrEqualTo(0);
        RuleFor(x => x.TareWeight).GreaterThanOrEqualTo(0);
        RuleFor(x => x.NetWeight).GreaterThanOrEqualTo(0);

        RuleFor(x => x.Images)
            .Must(imgs => imgs.Count <= 20)
            .WithMessage("Maximum 20 images per request.");

        RuleForEach(x => x.Images)
            .Must(f => f.Length <= 10 * 1024 * 1024)
            .WithMessage("Each image must not exceed 10 MB.");
    }
}
