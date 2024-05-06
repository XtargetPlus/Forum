using System.ComponentModel.DataAnnotations;

namespace Forum.Storage.Models;

public class DomainEvent
{
    [Key]
    public Guid Id { get; set; }

    public DateTimeOffset EmittedAt { get; set; }

    [Required]
    public byte[] ContentBlob { get; set; } = null!;
}