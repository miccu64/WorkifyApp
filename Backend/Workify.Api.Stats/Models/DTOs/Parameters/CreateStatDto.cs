namespace Workify.Api.Stats.Models.DTOs.Parameters
{
    internal record CreateStatDto(DateTime Time, double Weight, int Reps, string? Note);
}
