namespace APIMASTER.Models.Requests;

public class UpdateMilkLoadImagesOthersRequest
{
    public int IdMilkLoad { get; set; }
    public List<IFormFile> Images { get; set; } = [];
}
