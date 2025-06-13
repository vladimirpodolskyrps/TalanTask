using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using TalanTask.DTOs;
using TalanTask.Models;
using TalanTask.Services.Interfaces;
using TalanTask.Settings;

namespace TalanTask.Services;

public class HackerNewsStoriesService : IHackerNewsStoriesService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private readonly HackerNewsSettings _settings;

    public HackerNewsStoriesService(HttpClient httpClient, IMemoryCache cache,
        IOptions<HackerNewsSettings> settings)
    {
        _httpClient = httpClient;
        _cache = cache;
        _settings = settings.Value;
    }

    public async Task<IEnumerable<HackerNewsStoryModel>> GetTopStoriesAsync(int count)
    {
        var storyIds = await _httpClient.GetFromJsonAsync<List<int>>($"{_settings.BaseUrl}/{_settings.Endpoints.BestStories}");

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
        var cacheKey = $"story:{id}";

        if (_cache.TryGetValue(cacheKey, out HackerNewsStoryModel cachedStory))
        {
            return cachedStory;
        }

        try
        {
            var story = await _httpClient.GetFromJsonAsync<HackerNewsStory>(
                string.Format($"{_settings.BaseUrl}/{string.Format(_settings.Endpoints.StoryById, id)}", id));

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

            _cache.Set(cacheKey, result, TimeSpan.FromMinutes(5));

            return result;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
