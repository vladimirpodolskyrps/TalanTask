using Microsoft.AspNetCore.Mvc;
using TalanTask.Services.Interfaces;

namespace TalanTask.Controllers;

[ApiController]
[Route("api/stories")]
public class HackerNewsStoriesController : ControllerBase
{
    private readonly IHackerNewsStoriesService _hackerNewsService;

    public HackerNewsStoriesController(IHackerNewsStoriesService hackerNewsService)
    {
        _hackerNewsService = hackerNewsService;
    }

    public async Task<IActionResult> GetTopStories([FromQuery] int count = 5)
    {
        if (count <= 0)
        {
            return BadRequest("Count must be between > 0");
        }

        var stories = await _hackerNewsService.GetTopStoriesAsync(count);

        return Ok(stories);
    }
}

