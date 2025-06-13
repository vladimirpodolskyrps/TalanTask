namespace TalanTask.DTOs;

public class HackerNewsStory
{
    public string Title { get; set; } = null!;
    public string Url { get; set; } = null!;
    public string By { get; set; } = null!;
    public long Time { get; set; }
    public int Score { get; set; }
    public int Descendants { get; set; }
}
