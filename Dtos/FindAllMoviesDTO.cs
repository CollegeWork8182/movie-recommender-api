namespace movieRecommenderApi.Dtos;

public record FindAllMoviesDTO(long Id, string MovieTitle, List<string> Genres);