using Microsoft.EntityFrameworkCore;
using Storage.Models;

namespace Storage;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Topic> Topics { get; set; } = null!;
    public DbSet<Forum> Forums { get; set; } = null!;
    public DbSet<Session> Sessions { get; set; } = null!;
}