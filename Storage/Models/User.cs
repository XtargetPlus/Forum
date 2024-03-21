using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Storage.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }

    [MaxLength(20)]
    public string Login { get; set; } = null!;

    [MaxLength(100)]
    public byte[] Salt { get; set; } = null!;

    [MaxLength(32)]
    public byte[] PasswordHash { get; set; } = null!;

    [InverseProperty(nameof(Topic.Author))]
    public ICollection<Topic>? Topics { get; set; }

    [InverseProperty(nameof(Comment.Author))]
    public ICollection<Comment>? Comments { get; set; } 

    [InverseProperty(nameof(Session.User))]
    public ICollection<Session>? Sessions { get; set; }
}