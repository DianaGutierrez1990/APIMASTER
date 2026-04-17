namespace APIMASTER.Data.Entities;

public class Manager
{
    public int Id_Manager { get; set; }
    public string? Manager_Name { get; set; }
    public string? Email { get; set; }
    public string? Department { get; set; }
    public bool? Active { get; set; }
}

public class ApprovalDocument
{
    public int Id_Document { get; set; }
    public string? Document_Name { get; set; }
    public string? File_Name { get; set; }
    public string? File_Path { get; set; }
    public int? Id_User { get; set; }
    public DateTime? Created_At { get; set; }
}

public class ApprovalQuestion
{
    public int Id_Question { get; set; }
    public string? Question_Text { get; set; }
    public string? Answer_Text { get; set; }
    public string? Status { get; set; }
    public int? Id_Asked_By { get; set; }
    public string? Asked_By_Name { get; set; }
    public int? Id_Answer_By { get; set; }
    public string? Answer_By_Name { get; set; }
    public DateTime? Asked_At { get; set; }
    public DateTime? Answered_At { get; set; }
    public string? Comment { get; set; }
}
