using System.ComponentModel.DataAnnotations;

namespace Forum.API.Dtos.Requests;

public class CreateForumRequest
{
    [Required]
    public string Title { get; set; } = null!;
}