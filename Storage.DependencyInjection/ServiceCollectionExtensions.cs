using System.Reflection;
using Domain.Authentication;
using Domain.UseCases.CreateForum;
using Domain.UseCases.CreateTopic;
using Domain.UseCases.GetForums;
using Domain.UseCases.GetTopics;
using Domain.UseCases.SignIn;
using Domain.UseCases.SignOn;
using Domain.UseCases.SignOut;
using Microsoft.Extensions.DependencyInjection;
using Storage.Storages;
using Microsoft.EntityFrameworkCore;

namespace Storage.DependencyInjection;

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
            .AddScoped<IMomentProvider, MomentProvider>()
            .AddScoped<IGuidFactory, GuidFactory>()
            .AddDbContextPool<AppDbContext>(opt =>
            {
                opt.UseNpgsql(
                    dbConnectionString,
                    postOpt => { postOpt.MigrationsAssembly("Storage"); });
            });

        services.AddMemoryCache();

        services.AddAutoMapper(config => config.AddMaps(Assembly.GetAssembly(typeof(AppDbContext))));

        return services;
    }
}