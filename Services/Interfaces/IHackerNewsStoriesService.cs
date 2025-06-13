using TalanTask.Models;

namespace TalanTask.Services.Interfaces;

public interface IHackerNewsStoriesService
{
    Task<IEnumerable<HackerNewsStoryModel>> GetTopStoriesAsync(int count);
}
