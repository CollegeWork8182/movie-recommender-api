using movieRecommenderApi.Dtos;
using movieRecommenderApi.Entities;
using movieRecommenderApi.Infra;
using movieRecommenderApi.Interfaces;

namespace movieRecommenderApi.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;

    public MovieService(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<IEnumerable<MovieEntity>> RecommendAsync(string movieTitle, List<string> movieGenre, int k = 3)
    {
        // Cria um filme "virtual" com base na entrada
        var target = new MovieEntity()
        {
            Title = movieTitle,
            Genres = movieGenre
        };

        var movies = await _movieRepository.GetAllAsync();

        // Calcula a similaridade combinada (por gênero + nome)
        var similarities = movies
            .Where(m => !m.Title.Equals(target.Title, StringComparison.OrdinalIgnoreCase))
            .Select(m => new
            {
                Movie = m,
                Similarity = CalculateSimilarity(target, m)
            })
            .OrderByDescending(x => x.Similarity)
            .Take(k)
            .Select(x => x.Movie);

        return similarities;
    }

    public async Task<List<FindAllMoviesDTO>> FindAllAsync()
    {
        var movies = await _movieRepository.GetAllAsync();

        return movies
            .Select(m => new FindAllMoviesDTO(m.Id, m.Title, m.Genres))
            .ToList();
    }

    private double CalculateSimilarity(MovieEntity a, MovieEntity b)
    {
        // Similaridade de gêneros (Jaccard)
        double genreSim = CalculateJaccardSimilarity(a.Genres, b.Genres);

        // Similaridade de título (baseada na sobreposição de palavras)
        double titleSim = CalculateTitleSimilarity(a.Title, b.Title);

        // Combina as duas (peso: 70% gênero + 30% título)
        return (genreSim * 0.7) + (titleSim * 0.3);
    }

    private double CalculateJaccardSimilarity(List<string> a, List<string> b)
    {
        if (!a.Any() || !b.Any()) return 0;
        var intersection = a.Intersect(b, StringComparer.OrdinalIgnoreCase).Count();
        var union = a.Union(b, StringComparer.OrdinalIgnoreCase).Count();
        return union == 0 ? 0 : (double)intersection / union;
    }

    private double CalculateTitleSimilarity(string a, string b)
    {
        var aWords = a.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var bWords = b.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var intersection = aWords.Intersect(bWords).Count();
        var union = aWords.Union(bWords).Count();
        return union == 0 ? 0 : (double)intersection / union;
    }
}