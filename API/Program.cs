using TvMazeScraper;
using TvMazeScraper.Services;

var builder = WebApplication.CreateBuilder(args);

var app = builder.ConfigureServices();
app = app.ConfigurePipeline();
app.Run();
