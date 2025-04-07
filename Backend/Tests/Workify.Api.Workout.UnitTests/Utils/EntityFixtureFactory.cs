using AutoFixture;
using AutoFixture.Kernel;
using Workify.Api.Workout.Models.Entities;
using Workify.Api.Workout.Models.Entities.Abstractions;

namespace Workify.Api.Workout.UnitTests.Utils
{
    internal static class EntityFixtureFactory
    {
        public static Fixture Create()
        {
            Fixture fixture = new();
            fixture.Customizations.Add(new TypeRelay(typeof(Exercise), typeof(PredefinedExercise)));

            return fixture;
        }
    }
}
