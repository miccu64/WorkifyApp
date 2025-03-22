using System.Reflection;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Workify.Api.Auth.Database;
using Workify.Api.Auth.Services;
using Workify.Utils.Config;
using Workify.Utils.Extensions;

var builder = WebApplication.CreateBuilder(args);

CommonConfig config = builder.CommonApiInitialization<CommonConfig>();

builder.Services.AddDbContext<AuthDbContext>(opt => opt.UseNpgsql(config.DbConnectionString));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthDbContext>(provider => provider.GetService<AuthDbContext>()!);

builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetService<AuthDbContext>()!;
    context.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.CommonApiInitialization();
app.MapControllers();

app.Run();
