using System.Reflection;
using FluentValidation;
using Forum.Domain.Authentication;
using Forum.Domain.Authorization;
using Forum.Domain.Authorization.AccessManagement;
using Forum.Domain.Monitoring;
using Microsoft.Extensions.DependencyInjection;

namespace Forum.Domain.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddForumDomain(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg
            .AddOpenBehavior(typeof(MonitoringPipelineBehavior<,>))
            .AddOpenBehavior(typeof(ValidationPipelineBehavior<,>))
            .RegisterServicesFromAssembly(Assembly.Load("Forum.Domain")));

        services
            .AddScoped<IIntentionManager, IntentionManager>()
            .AddScoped<IIntentionResolver, ForumIntentionResolver>()
            .AddScoped<IIntentionResolver, TopicIntentionResolver>()
            .AddScoped<IIntentionResolver, AccountIntentionResolver>()
            .AddScoped<IIntentionResolver, CommentIntentionResolver>();

        services
            .AddScoped<ISymmetricDecryptor, AesSymmetricEncryptorDecryptor>()
            .AddScoped<ISymmetricEncryptor, AesSymmetricEncryptorDecryptor>()
            .AddScoped<IAuthenticationService, AuthenticationService>()
            .AddScoped<IPasswordManager, PasswordManager>()
            .AddScoped<IIdentityProvider, IdentityProvider>();

        services.AddValidatorsFromAssembly(Assembly.Load("Forum.Domain"), includeInternalTypes: true);

        services.AddSingleton<DomainMetrics>();

        return services;
    }
}