//Ler os files que vem da API
using System.Text.Json.Serialization;
namespace movieRecommenderApi.Dtos.Tmdb;

// Representa a resposta da lista de filmes (Top Rated ou Popular)
public class TmdbMovieResponse
{
    [JsonPropertyName("results")]
    public List<TmdbMovieResult> Results { get; set; } = new();
}

public class TmdbMovieResult
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    // O TMDb devolve IDs de genero (ex: [28, 12]), não os nomes
    [JsonPropertyName("genre_ids")]
    public List<int> GenreIds { get; set; } = new();
}