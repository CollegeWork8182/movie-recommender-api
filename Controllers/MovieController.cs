using Microsoft.AspNetCore.Mvc;
using movieRecommenderApi.Dtos;
using movieRecommenderApi.Infra;
using movieRecommenderApi.Services;

namespace movieRecommenderApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{
    private readonly MovieService _movieService;
    private readonly MockMovies _mockMovies;

    public MovieController()
    {
        _mockMovies = new MockMovies();
        _movieService = new MovieService(_mockMovies);
    }

    [HttpPost("recommender")]
    public IActionResult GetRecommendations([FromBody] MovieRequestDTO data)
    {
        var recommendations = _movieService.Recommend(data.Title, data.Genres);

        if (!recommendations.Any())
            return NotFound(new { message = "Filme não encontrado" });

        return Ok(recommendations);
    }

    [HttpGet("all")]
    public IActionResult GetAll()
    {
        return Ok(_movieService.FindAll());
    }
    
}