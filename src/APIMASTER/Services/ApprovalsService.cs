using Dapper;
using APIMASTER.Data.Entities;
using APIMASTER.Models.Requests;

namespace APIMASTER.Services;

public class ApprovalsService : IApprovalsService
{
    private readonly IDatabaseResolver _dbResolver;
    private readonly IFileStorageService _fileStorage;
    private readonly ILogger<ApprovalsService> _logger;

    public ApprovalsService(IDatabaseResolver dbResolver, IFileStorageService fileStorage, ILogger<ApprovalsService> logger)
    {
        _dbResolver = dbResolver;
        _fileStorage = fileStorage;
        _logger = logger;
    }

    public async Task<IEnumerable<Manager>> GetManagersAsync()
    {
        const string sql = "SELECT * FROM Managers WHERE Active = 1 ORDER BY Manager_Name ASC";

        using var conn = _dbResolver.GetConnectionByName("ApprovalProd");
        await conn.OpenAsync();
        return await conn.QueryAsync<Manager>(sql);
    }

    public async Task<int> SaveDocumentAsync(SaveDocumentRequest request)
    {
        // 1. Save file
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.File.FileName)}";
        await _fileStorage.SaveFileAsync(request.File, "approvals", "documents", fileName);

        // 2. Register in DB
        using var conn = _dbResolver.GetConnectionByName("ApprovalProd");
        await conn.OpenAsync();

        var docId = await conn.ExecuteScalarAsync<int>(
            @"INSERT INTO Documents (Document_Name, File_Name, File_Path, Id_User, Created_At)
              VALUES (@DocumentName, @FileName, @FilePath, @IdUser, GETUTCDATE());
              SELECT SCOPE_IDENTITY();",
            new
            {
                DocumentName = request.DocumentName,
                FileName = fileName,
                FilePath = $"approvals/documents/{fileName}",
                IdUser = request.IdUser
            });

        _logger.LogInformation("Document {DocId} saved by user {UserId}", docId, request.IdUser);
        return docId;
    }

    public async Task<int> StoreQuestionAsync(StoreQuestionRequest request)
    {
        using var conn = _dbResolver.GetConnectionByName("ApprovalProd");
        await conn.OpenAsync();

        var questionId = await conn.ExecuteScalarAsync<int>(
            @"INSERT INTO Questions (Question_Text, Status, Id_Asked_By, Id_Answer_By, Asked_At)
              VALUES (@QuestionText, 'pending', @IdAskedBy, @IdAnswerBy, GETUTCDATE());
              SELECT SCOPE_IDENTITY();",
            new
            {
                QuestionText = request.QuestionText,
                IdAskedBy = request.IdAskedBy,
                IdAnswerBy = request.IdAnswerBy
            });

        _logger.LogInformation("Question {QuestionId} created by user {UserId}", questionId, request.IdAskedBy);
        return questionId;
    }

    public async Task<(IEnumerable<ApprovalQuestion> Items, int TotalCount)> GetAskedQuestionsAsync(
        int userId, int skip, int take)
    {
        const string sql = @"
            SELECT q.*,
                   asker.Manager_Name AS Asked_By_Name,
                   answerer.Manager_Name AS Answer_By_Name
            FROM Questions q
            LEFT JOIN Managers asker ON q.Id_Asked_By = asker.Id_Manager
            LEFT JOIN Managers answerer ON q.Id_Answer_By = answerer.Id_Manager
            WHERE q.Id_Asked_By = @UserId
            ORDER BY q.Asked_At DESC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;

            SELECT COUNT(*) FROM Questions WHERE Id_Asked_By = @UserId;";

        using var conn = _dbResolver.GetConnectionByName("ApprovalProd");
        await conn.OpenAsync();
        using var multi = await conn.QueryMultipleAsync(sql, new { UserId = userId, Skip = skip, Take = take });

        var items = await multi.ReadAsync<ApprovalQuestion>();
        var totalCount = await multi.ReadSingleAsync<int>();
        return (items, totalCount);
    }

    public async Task<(IEnumerable<ApprovalQuestion> Items, int TotalCount)> GetPendingQuestionsAsync(
        int userId, int skip, int take)
    {
        const string sql = @"
            SELECT q.*,
                   asker.Manager_Name AS Asked_By_Name,
                   answerer.Manager_Name AS Answer_By_Name
            FROM Questions q
            LEFT JOIN Managers asker ON q.Id_Asked_By = asker.Id_Manager
            LEFT JOIN Managers answerer ON q.Id_Answer_By = answerer.Id_Manager
            WHERE q.Id_Answer_By = @UserId AND q.Status = 'pending'
            ORDER BY q.Asked_At DESC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;

            SELECT COUNT(*) FROM Questions WHERE Id_Answer_By = @UserId AND Status = 'pending';";

        using var conn = _dbResolver.GetConnectionByName("ApprovalProd");
        await conn.OpenAsync();
        using var multi = await conn.QueryMultipleAsync(sql, new { UserId = userId, Skip = skip, Take = take });

        var items = await multi.ReadAsync<ApprovalQuestion>();
        var totalCount = await multi.ReadSingleAsync<int>();
        return (items, totalCount);
    }

    public async Task ChangeStatusAsync(ChangeStatusRequest request)
    {
        using var conn = _dbResolver.GetConnectionByName("ApprovalProd");
        await conn.OpenAsync();

        var affected = await conn.ExecuteAsync(
            @"UPDATE Questions
              SET Status = @Status, Comment = @Comment, Answered_At = GETUTCDATE()
              WHERE Id_Question = @IdQuestion",
            new
            {
                Status = request.Status,
                Comment = request.Comment,
                IdQuestion = request.IdQuestion
            });

        if (affected == 0)
            throw new KeyNotFoundException($"Question {request.IdQuestion} not found.");

        _logger.LogInformation("Question {QuestionId} status changed to {Status} by user {UserId}",
            request.IdQuestion, request.Status, request.IdUser);
    }
}
