namespace TalanTask.Models;

public class HackerNewsStoryModel
{
    public string Title { get; set; } = null!;
    public string Uri { get; set; } = null!;
    public string PostedBy { get; set; } = null!;
    public DateTime Time { get; set; }
    public int Score { get; set; }
    public int CommentCount { get; set; }
}
