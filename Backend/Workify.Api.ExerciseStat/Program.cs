using FluentValidation;

using MassTransit;

using Microsoft.EntityFrameworkCore;

using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

using Workify.Api.ExerciseStat.Communication.Consumers;
using Workify.Api.ExerciseStat.Database;
using Workify.Api.ExerciseStat.Models.DTOs.Parameters;
using Workify.Api.ExerciseStat.Services;
using Workify.Utils.Config;
using Workify.Utils.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

CommonConfig config = builder.CommonApiInitialization<CommonConfig>();

builder.Services.AddDbContext<StatDbContext>(opt => opt.UseNpgsql(config.DbConnectionString));

builder.Services.AddValidatorsFromAssemblyContaining<CreateEditStatDtoValidator>(includeInternalTypes: true);
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddScoped<IStatService, StatService>();
builder.Services.AddScoped<IStatDbContext>(provider => provider.GetService<StatDbContext>()!);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<DeletedExerciseConsumer>();
    x.AddConsumer<DeletedUserConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(config.RabbitMqHostname, "/", h =>
        {
            h.Username(config.RabbitMqUsername);
            h.Password(config.RabbitMqPassword);
        });

        cfg.ConfigureEndpoints(context);
    });
});

WebApplication app = builder.Build();

using (IServiceScope serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    StatDbContext context = serviceScope.ServiceProvider.GetService<StatDbContext>()!;
    context.Database.Migrate();
}

app.CommonApiInitialization();

app.Run();
