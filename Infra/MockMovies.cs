using movieRecommenderApi.Entities;
using movieRecommenderApi.Interfaces;

namespace movieRecommenderApi.Infra;

public class MockMovies : IMovieRepository
{
    public ICollection<MovieEntity> Movies { get; set; }

    public MockMovies()
    {
        Movies = new List<MovieEntity>
        {
            new MovieEntity { Id = 1, Title = "Matrix", Genres = new() { "Action", "Sci-Fi" } },
            new MovieEntity { Id = 2, Title = "Inception", Genres = new() { "Action", "Thriller", "Sci-Fi" } },
            new MovieEntity { Id = 3, Title = "Interstellar", Genres = new() { "Adventure", "Drama", "Sci-Fi" } },
            new MovieEntity { Id = 4, Title = "The Dark Knight", Genres = new() { "Action", "Crime", "Drama" } },
            new MovieEntity { Id = 5, Title = "Tenet", Genres = new() { "Action", "Sci-Fi", "Thriller" } },
            new MovieEntity { Id = 6, Title = "Avengers", Genres = new() { "Action", "Adventure", "Sci-Fi" } },
            new MovieEntity { Id = 7, Title = "Joker", Genres = new() { "Crime", "Drama", "Thriller" } }
        };
    }

    public Task<ICollection<MovieEntity>> GetAllAsync()
    {
        return Task.FromResult(Movies);
    }
}