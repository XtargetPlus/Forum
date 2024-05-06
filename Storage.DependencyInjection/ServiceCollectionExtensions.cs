using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Forum.Domain;
using Forum.Domain.UseCases;
using Forum.Domain.UseCases.SignOn;
using Forum.Domain.UseCases.CreateTopic;
using Forum.Domain.UseCases.CreateForum;
using Forum.Domain.UseCases.SignOut;
using Forum.Domain.UseCases.GetTopics;
using Forum.Domain.UseCases.GetForums;
using Forum.Domain.UseCases.SignIn;
using Forum.Domain.Authentication;
using Forum.Storage.Storages;

namespace Forum.Storage.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddForumStorage(this IServiceCollection services, string dbConnectionString)
    {
        services
            .AddScoped<IAuthenticationStorage, AuthenticationStorage>()
            .AddScoped<ISignOnStorage, SignOnStorage>()
            .AddScoped<ISignInStorage, SignInStorage>()
            .AddScoped<ISignOutStorage, SignOutStorage>()
            .AddScoped<IGetForumsStorage, GetForumsStorage>()
            .AddScoped<ICreateForumStorage, CreateForumStorage>()
            .AddScoped<ICreateTopicStorage, CreateTopicStorage>()
            .AddScoped<IGetTopicsStorage, GetTopicsStorage>()
            .AddScoped<IDomainEventStorage, DomainEventStorage>()
            .AddScoped<IGuidFactory, GuidFactory>()
            .AddDbContextPool<AppDbContext>(opt =>
            {
                opt.UseNpgsql(
                    dbConnectionString,
                    postOpt => { postOpt.MigrationsAssembly("Storage"); });
            });

        services
            .AddSingleton(TimeProvider.System)
            .AddSingleton<IUnitOfWork>(p => new UnitOfWork(p));

        services.AddMemoryCache();

        services.AddAutoMapper(config => config.AddMaps(Assembly.GetAssembly(typeof(AppDbContext))));

        return services;
    }
}