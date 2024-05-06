using System.Reflection;
using FluentValidation;
using Forum.Domain.Authentication;
using Forum.Domain.Authorization;
using Forum.Domain.Monitoring;
using Forum.Domain.UseCases.CreateForum;
using Forum.Domain.UseCases.CreateTopic;
using Forum.Domain.UseCases.SignOut;
using Microsoft.Extensions.DependencyInjection;

namespace Forum.Domain.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddForumDomain(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg
            .AddOpenBehavior(typeof(MonitoringPipelineBehavior<,>))
            .AddOpenBehavior(typeof(ValidationPipelineBehavior<,>))
            .RegisterServicesFromAssembly(Assembly.Load("Domain")));

        services
            .AddScoped<IIntentionManager, IntentionManager>()
            .AddScoped<IIntentionResolver, ForumIntentionResolver>()
            .AddScoped<IIntentionResolver, TopicIntentionResolver>()
            .AddScoped<IIntentionResolver, AccountIntentionResolver>();

        services
            .AddScoped<ISymmetricDecryptor, AesSymmetricEncryptorDecryptor>()
            .AddScoped<ISymmetricEncryptor, AesSymmetricEncryptorDecryptor>()
            .AddScoped<IAuthenticationService, AuthenticationService>()
            .AddScoped<IPasswordManager, PasswordManager>()
            .AddScoped<IIdentityProvider, IdentityProvider>();

        services.AddValidatorsFromAssembly(Assembly.Load("Domain"), includeInternalTypes: true);

        services.AddSingleton<DomainMetrics>();

        return services;
    }
}