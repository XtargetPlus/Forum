using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Storage.Entities;

public class Comment
{
    [Key]
    public Guid Id { get; set; }

    [MaxLength(255)]
    public string Text { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? Author { get; set; }

    public Guid TopicId { get; set; }

    [ForeignKey(nameof(TopicId))]
    public Topic? Topic { get; set; }
}
