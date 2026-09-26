using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Workify.Api.Auth.Config;
using Workify.Api.Auth.Database;
using Workify.Api.Auth.Models.DTOs;
using Workify.Api.Auth.Services;
using Workify.Utils.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

AuthConfig config = builder.CommonApiInitialization<AuthConfig>();

builder.Services.AddDbContext<AuthDbContext>(opt => opt.UseNpgsql(config.DbConnectionString));

builder.Services.AddValidatorsFromAssemblyContaining<LogInDtoValidator>(includeInternalTypes: true);
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserManagementService, UserManagementService>();
builder.Services.AddScoped<IAuthDbContext>(provider => provider.GetService<AuthDbContext>()!);

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((_, cfg) =>
    {
        cfg.Host(config.RabbitMqHostname, config.RabbitMqPort, "/", h =>
        {
            h.Username(config.RabbitMqUsername);
            h.Password(config.RabbitMqPassword);
        });
    });

    x.AddConfigureEndpointsCallback((_, _, cfg) =>
    {
        cfg.UseMessageRetry(r => r.Interval(5, 100));
    });
});

WebApplication app = builder.Build();

using (IServiceScope serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    AuthDbContext context = serviceScope.ServiceProvider.GetService<AuthDbContext>()!;
    context.Database.Migrate();
}

app.CommonApiInitialization();

app.Run();