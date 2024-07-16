using System.ComponentModel.DataAnnotations;

namespace Forum.API.Dtos.Requests;

public class CreateTopicRequest
{
    [Required]
    public string Title { get; set; } = null!;
}