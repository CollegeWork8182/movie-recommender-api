using movieRecommenderApi.Dtos;
using movieRecommenderApi.Entities;
using movieRecommenderApi.Infra;

namespace movieRecommenderApi.Services;

public class MovieService
{
    private MockMovies _mockMovies;

    public MovieService(MockMovies mockMovies)
    {
        _mockMovies = mockMovies;
    }

    public IEnumerable<MovieEntity> Recommend(string movieTitle, List<string> movieGenre, int k = 3)
    {
        // Cria um filme "virtual" com base na entrada
        var target = new MovieEntity()
        {
            Title = movieTitle,
            Genres = movieGenre
        };

        // Calcula a similaridade combinada (por gênero + nome)
        var similarities = _mockMovies.Movies
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

    public List<FindAllMoviesDTO> FindAll()
    {
        var movies = _mockMovies.Movies
            .Select(m => new FindAllMoviesDTO(m.Id, m.Title, m.Genres))
            .ToList();

        return movies;
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