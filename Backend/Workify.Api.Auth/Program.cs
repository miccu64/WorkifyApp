using FluentValidation;

using Microsoft.EntityFrameworkCore;

using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

using Workify.Api.Auth.Database;
using Workify.Api.Auth.Models.DTOs;
using Workify.Api.Auth.Services;
using Workify.Utils.Config;
using Workify.Utils.Extensions;

var builder = WebApplication.CreateBuilder(args);

CommonConfig config = builder.CommonApiInitialization<CommonConfig>();

builder.Services.AddDbContext<AuthDbContext>(opt => opt.UseNpgsql(config.DbConnectionString));

builder.Services.AddValidatorsFromAssemblyContaining<LogInDtoValidator>(includeInternalTypes: true);
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthDbContext>(provider => provider.GetService<AuthDbContext>()!);

var app = builder.Build();

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetService<AuthDbContext>()!;
    context.Database.Migrate();
}

app.CommonApiInitialization();

app.Run();
