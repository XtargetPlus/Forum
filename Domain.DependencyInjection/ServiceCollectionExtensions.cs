using System.Reflection;
using Domain.Authentication;
using Domain.Authorization;
using Domain.Monitoring;
using Domain.UseCases;
using Domain.UseCases.CreateForum;
using Domain.UseCases.CreateTopic;
using Domain.UseCases.SignOut;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.DependencyInjection;

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