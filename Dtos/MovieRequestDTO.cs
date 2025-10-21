namespace movieRecommenderApi.Dtos;

public record MovieRequestDTO(string Title, List<string> Genres);