using System.ComponentModel.DataAnnotations;

namespace Forum.API.Models.Requests;

public class CreateTopicRequest
{
    [Required]
    public string Title { get; set; } = null!;
}