using System.ComponentModel.DataAnnotations;

namespace Forum.API.Models.Requests;

public class CreateForumRequest
{
    [Required]
    public string Title { get; set; } = null!;
}