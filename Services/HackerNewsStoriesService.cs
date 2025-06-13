using Microsoft.Extensions.Caching.Memory;
using TalanTask.DTOs;
using TalanTask.Models;
using TalanTask.Services.Interfaces;

namespace TalanTask.Services;

public class HackerNewsStoriesService : IHackerNewsStoriesService
{
    private const string BestStoriesUrl = "https://hacker-news.firebaseio.com/v0/beststories.json";
    private const string StoryDetailUrl = "https://hacker-news.firebaseio.com/v0/item/{0}.json";

    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;

    public HackerNewsStoriesService(HttpClient httpClient, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _cache = cache;
    }

    public async Task<IEnumerable<HackerNewsStoryModel>> GetTopStoriesAsync(int count)
    {
        var storyIds = await _httpClient.GetFromJsonAsync<List<int>>(BestStoriesUrl);

        if (storyIds is null)
        {
            return Enumerable.Empty<HackerNewsStoryModel>();
        }

        var tasks = storyIds.Select(GetStoryAsync);

        var stories = await Task.WhenAll(tasks);

        return stories.Where(s => s is not null).OrderByDescending(s => s.Score).Take(count);
    }

    private async Task<HackerNewsStoryModel?> GetStoryAsync(int id)
    {
        string cacheKey = $"story:{id}";

        if (_cache.TryGetValue(cacheKey, out HackerNewsStoryModel cachedStory))
        {
            return cachedStory;
        }

        try
        {
            var story = await _httpClient.GetFromJsonAsync<HackerNewsStory>(string.Format(StoryDetailUrl, id));

            if (story is null)
            {
                return null;
            }

            var result = new HackerNewsStoryModel
            {
                Title = story.Title,
                Uri = story.Url,
                PostedBy = story.By,
                Time = DateTimeOffset.FromUnixTimeSeconds(story.Time).UtcDateTime,
                Score = story.Score,
                CommentCount = story.Descendants
            };

            _cache.Set(cacheKey, result, TimeSpan.FromMinutes(5)); // I would say it depends on some factors what is duration number should be

            return result;
        }
        catch (Exception)
        {
            // Logging may be added here if needed
            return null;
        }
    }
}
