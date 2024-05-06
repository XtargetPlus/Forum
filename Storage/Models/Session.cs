using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Storage.Models;

public class Session
{
    [Key]
    public Guid Id { get; set; }

    public DateTimeOffset ExpiresAt { get; set; }

    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
}