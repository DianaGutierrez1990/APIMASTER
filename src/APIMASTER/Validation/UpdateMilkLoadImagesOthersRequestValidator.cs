using APIMASTER.Models.Requests;
using FluentValidation;

namespace APIMASTER.Validation;

public class UpdateMilkLoadImagesOthersRequestValidator : AbstractValidator<UpdateMilkLoadImagesOthersRequest>
{
    public UpdateMilkLoadImagesOthersRequestValidator()
    {
        RuleFor(x => x.IdMilkLoad).GreaterThan(0);

        RuleFor(x => x.Images)
            .NotEmpty().WithMessage("At least one image is required.")
            .Must(imgs => imgs.Count <= 20)
            .WithMessage("Maximum 20 images per request.");

        RuleForEach(x => x.Images)
            .Must(f => f.Length <= 10 * 1024 * 1024)
            .WithMessage("Each image must not exceed 10 MB.");
    }
}
