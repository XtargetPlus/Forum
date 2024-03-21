using System.Reflection;
using Domain.Authentication;
using Domain.Authorization;
using Domain.Monitoring;
using Domain.UseCases.CreateForum;
using Domain.UseCases.CreateTopic;
using Domain.UseCases.GetForums;
using Domain.UseCases.GetTopics;
using Domain.UseCases.SignIn;
using Domain.UseCases.SignOn;
using Domain.UseCases.SignOut;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddForumDomain(this IServiceCollection services)
    {
        services
            .AddScoped<ISignOnUseCase, SignOnUseCase>()
            .AddScoped<ISignInUseCase, SignInUseCase>()
            .AddScoped<ICreateForumUseCase, CreateForumUseCase>()
            .AddScoped<IGetForumsUseCase, GetForumsUseCase>()
            .AddScoped<ICreateTopicUseCase, CreateTopicUseCase>()
            .AddScoped<IGetTopicsUseCase, GetTopicsUseCase>()
            .AddScoped<IIntentionResolver, ForumIntentionResolver>()
            .AddScoped<IIntentionResolver, TopicIntentionResolver>()
            .AddScoped<IIntentionResolver, AccountIntentionResolver>()
            .AddScoped<ISignOutUseCase, SignOutUseCase>();

        services
            .AddScoped<ISymmetricDecryptor, AesSymmetricEncryptorDecryptor>()
            .AddScoped<ISymmetricEncryptor, AesSymmetricEncryptorDecryptor>()
            .AddScoped<IAuthenticationService, AuthenticationService>()
            .AddScoped<IPasswordManager, PasswordManager>()
            .AddScoped<IIntentionManager, IntentionManager>()
            .AddScoped<IIdentityProvider, IdentityProvider>();

        services.AddValidatorsFromAssembly(Assembly.Load("Domain"), includeInternalTypes: true);

        services.AddSingleton<DomainMetrics>();

        return services;
    }
}