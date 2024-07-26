using System.ComponentModel.DataAnnotations;

namespace Forum.Storage.Entities;

public class DomainEvent
{
    [Key]
    public Guid Id { get; set; }

    public DateTimeOffset EmittedAt { get; set; }

    [MaxLength(55)]
    public string? ActivityId { get; set; }

    [Required]
    public byte[] ContentBlob { get; set; } = null!;
}