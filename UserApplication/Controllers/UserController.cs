using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;

[ApiController]
[Route("api/[controller]")]
public class UserFileUploadController : ControllerBase
{
    private readonly IDataStorageService _storageService;

    public UserFileUploadController(IDataStorageService storageService)
    {
        _storageService = storageService;
    }

    [HttpPost(Name ="SaveUserData")]
    public async Task<IActionResult> SaveUserData([FromBody] List<UserRequest> requests)
    {
        if (requests == null || requests.Count == 0)
        {
            return BadRequest("Request list cannot be empty.");
        }

        foreach (var request in requests)
        {
            var context = new ValidationContext(request, null, null);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(request, context, results, true))
            {
                return BadRequest(new
                {
                    Message = "Validation Failed",
                    Errors = results.Select(e => e.ErrorMessage)
                });
            }
        }

        string savedFile = await _storageService.StoreDataAsFileAsync(requests);
        return Ok(new { Message = "Saved successfully", Path = savedFile });
    }

}
public class UserRequest
{
    [Required(ErrorMessage = "ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0.")]
    public int? ID { get; set; }

    [Required(ErrorMessage = "UserID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "UserID must be greater than 0.")]
    public int? UserID { get; set; }

    [Required]
    [StringLength(50)]
    public string EmployeeID { get; set; }

    [Required]
    [StringLength(100)]
    public string SiteName { get; set; }

    [Required]
    [StringLength(100)]
    public string BusinessUnitName { get; set; }

    [Required]
    [StringLength(100)]
    public string AccountName { get; set; }

    [Required]
    [StringLength(100)]
    public string GroupName { get; set; }

    [Required]
    [StringLength(100)]
    public string CategoryName { get; set; }

    [Required]
    [StringLength(100)]
    public string TypeName { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    [RegularExpression(@"^\d{2}:\d{2}$", ErrorMessage = "Duration must be in HH:mm format.")]
    public string Duration { get; set; }

    public bool IsProcessed { get; set; }
}
public class DataStorageService : IDataStorageService
{
    private readonly IConfiguration _configuration;

    public DataStorageService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> StoreDataAsFileAsync(IEnumerable<UserRequest> requests)
    {
        string basePath = _configuration["FileSettings:BasePath"];
        if (basePath.Length == 0)
        {
            throw new ArgumentException("FileSettings basepath is not set");
        }
        string inDirectory = Path.Combine(basePath, "Users", "IN");

        if (!Directory.Exists(inDirectory))
        {
            Directory.CreateDirectory(inDirectory);
        }

        ///string fileName = $"user_request_{Guid.NewGuid()}.json";
        string fileName = "user_request".AppendTimeStamp();
        string filePath = Path.Combine(inDirectory, fileName);

        var json = JsonSerializer.Serialize(requests, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(filePath, json);

        return filePath;
    }
}
public interface IDataStorageService
{
    Task<string> StoreDataAsFileAsync(IEnumerable<UserRequest> requests);
}
public static class FileExtension
{
    public static string AppendTimeStamp(this string fileName)
    {
        return string.Concat(
            Path.GetFileNameWithoutExtension(fileName),
            DateTime.Now.ToString("_yyyyMMddHHmmss"),
            ".json"
            );
    }
}