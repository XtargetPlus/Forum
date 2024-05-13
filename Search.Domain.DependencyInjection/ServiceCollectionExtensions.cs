using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Search.Domain.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSearchDomain(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg
            .RegisterServicesFromAssembly(Assembly.Load("Search.Domain")));

        services.AddValidatorsFromAssembly(Assembly.Load("Search.Domain"), includeInternalTypes: true);

        return services;
    }
}