using movieRecommenderApi.Dtos.Tmdb;
using movieRecommenderApi.Entities;
using movieRecommenderApi.Interfaces;

namespace movieRecommenderApi.Infra;

public class TmdbRepository : IMovieRepository
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public TmdbRepository(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;

        // O "??" significa: Se for nulo, lance um erro agora (Fail Fast)
        var baseUrl = configuration["TmdbSettings:BaseUrl"]
                      ?? throw new ArgumentNullException("A URL base do TMDb não foi configurada.");

        _apiKey = configuration["TmdbSettings:ApiKey"]
                  ?? throw new ArgumentNullException("A API Key do TMDb não foi configurada.");

        _httpClient.BaseAddress = new Uri(baseUrl);
    }

    public async Task<ICollection<MovieEntity>> GetAllAsync()
    {
        // 1. Buscar a lista de gêneros para fazer o "De-Para" (ID -> Nome)
        var genresResponse = await _httpClient.GetFromJsonAsync<TmdbGenreResponse>($"genre/movie/list?api_key={_apiKey}&language=en-US");
        var genreMap = genresResponse?.Genres.ToDictionary(g => g.Id, g => g.Name) ?? new Dictionary<int, string>();

        var allMovies = new List<MovieEntity>();
        int pagesToFetch = 5;
        for (int page = 1; page <= pagesToFetch; page++)
        {
            try
            {
                // Passamos o parâmetro "&page={page}" dinâmico na URL
                var moviesResponse = await _httpClient.GetFromJsonAsync<TmdbMovieResponse>($"movie/top_rated?api_key={_apiKey}&language=en-US&page={page}");

                if (moviesResponse?.Results != null)
                {
                    foreach (var item in moviesResponse.Results)
                    {
                        // Converte IDs de gênero para Nomes
                        var genreNames = item.GenreIds
                            .Where(id => genreMap.ContainsKey(id))
                            .Select(id => genreMap[id])
                            .ToList();

                        allMovies.Add(new MovieEntity
                        {
                            Id = item.Id,
                            Title = item.Title,
                            Genres = genreNames
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                // Se der erro em uma página, continua para próxima
                Console.WriteLine($"Erro ao buscar página {page}: {ex.Message}");
            }
        }

        return allMovies;
    }
}