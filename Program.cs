using TalanTask.Services;
using TalanTask.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<IHackerNewsStoriesService, HackerNewsStoriesService>();

var app = builder.Build();

app.MapControllers();
app.Run();
