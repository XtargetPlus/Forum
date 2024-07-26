using Forum.Storage.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.Storage;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Topic> Topics { get; set; } = null!;
    public DbSet<Entities.Forum> Forums { get; set; } = null!;
    public DbSet<Session> Sessions { get; set; } = null!;
    public DbSet<DomainEvent> DomainEvents { get; set; } = null!;
}