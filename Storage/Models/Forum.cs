﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Storage.Models;

public class Forum
{
    [Key]
    public Guid Id { get; set; }

    [MaxLength(50)]
    public string Title { get; set; } = null!;

    [InverseProperty(nameof(Topic.Forum))]
    public ICollection<Topic>? Topics { get; set; }
}
