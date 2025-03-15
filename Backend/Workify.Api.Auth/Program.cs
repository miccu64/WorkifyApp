using Workify.Api.Auth.Database;
using Workify.Api.Auth.Services;
using Workify.Utils.Config;
using Workify.Utils.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.CommonApiInitialization<CommonConfig>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthDbContext>(provider => provider.GetService<AuthDbContext>()!);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
