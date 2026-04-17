namespace APIMASTER.Models.Requests;

public class SaveDocumentRequest
{
    public string DocumentName { get; set; } = string.Empty;
    public int IdUser { get; set; }
    public IFormFile File { get; set; } = null!;
}

public class StoreQuestionRequest
{
    public string QuestionText { get; set; } = string.Empty;
    public int IdAskedBy { get; set; }
    public int IdAnswerBy { get; set; }
}

public class ChangeStatusRequest
{
    public int IdQuestion { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Comment { get; set; }
    public int IdUser { get; set; }
}

public class QuestionsListRequest : PaginationParams
{
    public int UserId { get; set; }
}
