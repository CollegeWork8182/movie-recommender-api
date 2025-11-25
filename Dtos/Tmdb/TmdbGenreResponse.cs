//Ler a lista de gêneros, para traduzir ID -> Nome
using System.Text.Json.Serialization;
namespace movieRecommenderApi.Dtos.Tmdb;

public class TmdbGenreResponse
{
    [JsonPropertyName("genres")]
    public List<TmdbGenre> Genres { get; set; } = new();
}

public class TmdbGenre
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}