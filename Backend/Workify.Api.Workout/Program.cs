using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Workify.Api.Workout.Database;
using Workify.Api.Workout.Services;
using Workify.Utils.Config;
using Workify.Utils.Extensions;

var builder = WebApplication.CreateBuilder(args);

CommonConfig config = builder.CommonApiInitialization<CommonConfig>();

builder.Services.AddDbContext<WorkoutDbContext>(opt => opt.UseNpgsql(config.DbConnectionString));

//builder.Services.AddValidatorsFromAssemblyContaining<LogInDtoValidator>(includeInternalTypes: true);
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddScoped<IWorkoutDbContext, WorkoutDbContext>();
builder.Services.AddScoped<IWorkoutDbContext>(provider => provider.GetService<WorkoutDbContext>()!);
builder.Services.AddScoped<IPlanService, PlanService>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();

var app = builder.Build();

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetService<WorkoutDbContext>()!;
    context.Database.Migrate();
}

app.CommonApiInitialization();

app.Run();
