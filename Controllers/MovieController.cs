using Microsoft.AspNetCore.Mvc;
using movieRecommenderApi.Dtos;
using movieRecommenderApi.Infra;
using movieRecommenderApi.Interfaces;
using movieRecommenderApi.Services;

namespace movieRecommenderApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpPost("recommender")]
    public async Task<IActionResult> GetRecommendations([FromBody] MovieRequestDTO data)
    {
        var recommendations = await _movieService.RecommendAsync(data.Title, data.Genres);

        if (!recommendations.Any())
            return NotFound(new { message = "Filme não encontrado" });

        return Ok(recommendations);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _movieService.FindAllAsync());
    }
    
}