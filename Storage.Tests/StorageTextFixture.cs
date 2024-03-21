using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Testcontainers.PostgreSql;

namespace Storage.Tests;

public class StorageTextFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder().Build();
    
    public AppDbContext GetDbContext() => new(new DbContextOptionsBuilder<AppDbContext>()
        .UseNpgsql(_dbContainer.GetConnectionString()).Options);

    public IMapper GetMapper() => new Mapper(new MapperConfiguration(cfg => 
        cfg.AddMaps(Assembly.GetAssembly(typeof(AppDbContext)))));

    public IMemoryCache GetMemoryCache() => new MemoryCache(new MemoryCacheOptions());

    public virtual async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        var forumDbContext = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(_dbContainer.GetConnectionString()).Options);
        await forumDbContext.Database.MigrateAsync();
    }

    public async Task DisposeAsync() => await _dbContainer.DisposeAsync();
}