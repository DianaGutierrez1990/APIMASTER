using APIMASTER.Models.Requests;
using FluentValidation;

namespace APIMASTER.Validation;

public class UpsertMilkLoadRequestValidator : AbstractValidator<UpsertMilkLoadRequest>
{
    private static readonly string[] ValidImageTypes =
        ["manifest", "washtag", "thermometer", "dome", "load", "unload", "wash", "dump", "others"];

    public UpsertMilkLoadRequestValidator()
    {
        RuleFor(x => x.IdCustomerDairyBarnSilo).GreaterThan(0);
        RuleFor(x => x.IdUser).GreaterThan(0);
        RuleFor(x => x.GrossWeight).GreaterThanOrEqualTo(0);
        RuleFor(x => x.TareWeight).GreaterThanOrEqualTo(0);
        RuleFor(x => x.NetWeight).GreaterThanOrEqualTo(0);

        RuleFor(x => x.ImageType)
            .Must(t => string.IsNullOrEmpty(t) || ValidImageTypes.Contains(t.ToLowerInvariant()))
            .WithMessage($"ImageType must be one of: {string.Join(", ", ValidImageTypes)}");

        RuleFor(x => x.Image)
            .Must(f => f == null || f.Length <= 10 * 1024 * 1024)
            .WithMessage("Image size must not exceed 10 MB.");
    }
}
