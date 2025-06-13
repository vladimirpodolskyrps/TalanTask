using TalanTask.Services;
using TalanTask.Services.Interfaces;
using TalanTask.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<IHackerNewsStoriesService, HackerNewsStoriesService>();

builder.Services.Configure<HackerNewsSettings>(builder.Configuration.GetSection("HackerNews"));

var app = builder.Build();

app.MapControllers();
app.Run();
