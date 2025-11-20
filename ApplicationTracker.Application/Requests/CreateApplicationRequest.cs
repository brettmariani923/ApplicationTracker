using System.ComponentModel.DataAnnotations;

public class CreateApplicationRequest
{
    public string CompanyName { get; set; } = "";

    public string? JobTitle { get; set; }

    public int StageId { get; set; }  // <-- user’s “stage number”
}
