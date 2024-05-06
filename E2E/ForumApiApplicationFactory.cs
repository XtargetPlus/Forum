using System.Security.Cryptography;
using Forum.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;

namespace Forum.E2E;

public class ForumApiApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder().Build();

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        var forumDbContext = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(_dbContainer.GetConnectionString()).Options);
        await forumDbContext.Database.MigrateAsync();
    }

    public new async Task DisposeAsync() => await _dbContainer.DisposeAsync();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:Postgres"] = _dbContainer.GetConnectionString(),
                ["Authentication:Base64Key"] = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
            })
            .Build();

        builder.UseConfiguration(configuration);
        base.ConfigureWebHost(builder);
    }
}