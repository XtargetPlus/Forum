using AutoMapper;
using FluentAssertions;
using Forum.API;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Forum.E2E;

public class MapperConfigurationShould(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public void BeValid()
    {
        var configurationProvider = factory.Services.GetRequiredService<IMapper>().ConfigurationProvider;
        configurationProvider.Invoking(p => p.AssertConfigurationIsValid()).Should().NotThrow();
    }
}