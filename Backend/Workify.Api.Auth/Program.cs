using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Workify.Api.Auth.Database;
using Workify.Api.Auth.Models.DTOs;
using Workify.Api.Auth.Services;
using Workify.Utils.Config;
using Workify.Utils.Extensions;

var builder = WebApplication.CreateBuilder(args);

CommonConfig config = builder.CommonApiInitialization<CommonConfig>();

builder.Services.AddDbContext<AuthDbContext>(opt => opt.UseNpgsql(config.DbConnectionString));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthDbContext>(provider => provider.GetService<AuthDbContext>()!);

builder.Services.AddValidatorsFromAssembly(typeof(LogInDtoValidator).Assembly, includeInternalTypes: true);
builder.Services.AddControllers();

var app = builder.Build();

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetService<AuthDbContext>()!;
    context.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.CommonApiInitialization();
app.MapControllers();

app.Run();
