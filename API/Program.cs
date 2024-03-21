using System.Reflection;
using API.Authentication;
using API.Middlewares;
using API.Monitoring;
using Domain.Authentication;
using Domain.DependencyInjection;
using Storage.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiLogging(builder.Configuration, builder.Environment);
builder.Services.AddApiMetrics();
builder.Services.Configure<AuthenticationConfiguration>(builder.Configuration.GetSection("Authentication").Bind);
builder.Services.AddScoped<IAuthTokenStorage, AuthTokenStorage>();

builder.Services
    .AddForumDomain()
    .AddForumStorage(builder.Configuration.GetConnectionString("Postgres")!);

builder.Services.AddAutoMapper(config => config.AddMaps(Assembly.GetExecutingAssembly()));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
    
app.UseAuthorization();
    
app.MapControllers();
app.MapPrometheusScrapingEndpoint();

app
    .UseMiddleware<ErrorHandlingMiddleware>()
    .UseMiddleware<AuthenticationMiddleware>();

app.Run();

public partial class Program;