using APIMASTER.Models.Requests;
using FluentValidation;

namespace APIMASTER.Validation;

public class ScaleLoadsRequestValidator : AbstractValidator<ScaleLoadsRequest>
{
    public ScaleLoadsRequestValidator()
    {
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty();
    }
}

public class LoadCapturesRequestValidator : AbstractValidator<LoadCapturesRequest>
{
    public LoadCapturesRequestValidator()
    {
        RuleFor(x => x.IdWeight).NotEmpty().MaximumLength(50);
    }
}

public class ScaleImagesRequestValidator : AbstractValidator<ScaleImagesRequest>
{
    public ScaleImagesRequestValidator()
    {
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty();
    }
}

public class StoreImageRequestValidator : AbstractValidator<StoreImageRequest>
{
    public StoreImageRequestValidator()
    {
        RuleFor(x => x.IdCapture).NotEmpty().MaximumLength(50);
        RuleFor(x => x.IdWeight).NotEmpty().MaximumLength(50);
        RuleFor(x => x.ScaleTicket).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Bol).MaximumLength(50);
        RuleFor(x => x.Image).NotEmpty();
        RuleFor(x => x.DocumentType).NotEmpty().MaximumLength(50);
    }
}

public class StoreImagesBulkRequestValidator : AbstractValidator<StoreImagesBulkRequest>
{
    public StoreImagesBulkRequestValidator()
    {
        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Items array is required.")
            .Must(items => items.Count <= 10).WithMessage("Maximum 10 items per request.");

        RuleForEach(x => x.Items).SetValidator(new StoreImageItemValidator());
    }
}

public class StoreImageItemValidator : AbstractValidator<StoreImageItem>
{
    public StoreImageItemValidator()
    {
        RuleFor(x => x.IdCapture).NotEmpty().MaximumLength(50);
        RuleFor(x => x.IdWeight).NotEmpty().MaximumLength(50);
        RuleFor(x => x.ScaleTicket).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Bol).MaximumLength(50);
        RuleFor(x => x.Image).NotEmpty();
        RuleFor(x => x.DocumentType).NotEmpty().MaximumLength(50);
    }
}

public class StoreDocumentRequestValidator : AbstractValidator<StoreDocumentRequest>
{
    public StoreDocumentRequestValidator()
    {
        RuleFor(x => x.IdWeight).NotEmpty().MaximumLength(50);
        RuleFor(x => x.DocumentName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.DocumentPath).NotEmpty();
        RuleFor(x => x.DocumentExt).NotEmpty().MaximumLength(20);
    }
}

public class SavePulseRequestValidator : AbstractValidator<SavePulseRequest>
{
    public SavePulseRequestValidator()
    {
        RuleFor(x => x.IdScale).NotEmpty().MaximumLength(50);
        RuleFor(x => x.PulseDate).NotEmpty();
        RuleFor(x => x.Mac).NotEmpty().MaximumLength(50);
        RuleFor(x => x.PcName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Version).MaximumLength(50);
        RuleFor(x => x.Ip).MaximumLength(50);
    }
}
