using Microsoft.AspNetCore.Mvc;

using Workify.Api.ExerciseStat.Models.DTOs;
using Workify.Api.ExerciseStat.Services;

[ApiController]
[Route("api/stat")]
public class StatController(IStatService statService) : ControllerBase
{
    private readonly IStatService _statService = statService;


    [HttpGet("{userId}")]
    public async Task<IEnumerable<StatDto>> GetAllStats(int userId)
    {
        return await _statService.GetAllStats(userId);
    }
}