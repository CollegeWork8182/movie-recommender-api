using movieRecommenderApi.Dtos;
using movieRecommenderApi.Entities;

namespace movieRecommenderApi.Interfaces;

public interface IMovieService
{
    Task<IEnumerable<MovieEntity>> RecommendAsync(string movieTitle, List<string> movieGenre, int k = 3);
    Task<List<FindAllMoviesDTO>> FindAllAsync();
}
