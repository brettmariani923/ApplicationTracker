// ApplicationTracker.Application.Requests
using System.ComponentModel.DataAnnotations;

public class CreateApplicationRequest
{
    [Required]
    public string CompanyName { get; set; } = "";

    [Required]
    public string? JobTitle { get; set; }

    [Required]
    public int StageId { get; set; }  // <-- user’s “stage number”
}
