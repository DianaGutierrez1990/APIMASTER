using APIMASTER.Models.Requests;
using FluentValidation;

namespace APIMASTER.Validation;

public class SaveDocumentRequestValidator : AbstractValidator<SaveDocumentRequest>
{
    public SaveDocumentRequestValidator()
    {
        RuleFor(x => x.DocumentName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.IdUser).GreaterThan(0);
        RuleFor(x => x.File).NotNull().WithMessage("File is required.");
        RuleFor(x => x.File)
            .Must(f => f == null || f.Length <= 20 * 1024 * 1024)
            .WithMessage("File size must not exceed 20 MB.");
    }
}

public class StoreQuestionRequestValidator : AbstractValidator<StoreQuestionRequest>
{
    public StoreQuestionRequestValidator()
    {
        RuleFor(x => x.QuestionText).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.IdAskedBy).GreaterThan(0);
        RuleFor(x => x.IdAnswerBy).GreaterThan(0);
    }
}

public class ChangeStatusRequestValidator : AbstractValidator<ChangeStatusRequest>
{
    private static readonly string[] ValidStatuses = ["approved", "rejected", "pending"];

    public ChangeStatusRequestValidator()
    {
        RuleFor(x => x.IdQuestion).GreaterThan(0);
        RuleFor(x => x.IdUser).GreaterThan(0);
        RuleFor(x => x.Status)
            .NotEmpty()
            .Must(s => ValidStatuses.Contains(s.ToLowerInvariant()))
            .WithMessage($"Status must be one of: {string.Join(", ", ValidStatuses)}");
    }
}

public class QuestionsListRequestValidator : AbstractValidator<QuestionsListRequest>
{
    public QuestionsListRequestValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
    }
}
