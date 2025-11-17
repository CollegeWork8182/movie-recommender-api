using movieRecommenderApi.Entities;

namespace movieRecommenderApi.Interfaces;

public interface IMovieRepository
{
    Task<ICollection<MovieEntity>> GetAllAsync();
}
