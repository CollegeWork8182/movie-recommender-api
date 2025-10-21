namespace movieRecommenderApi.Entities;

public class MovieEntity
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<string> Genres { get; set; } = new();
}