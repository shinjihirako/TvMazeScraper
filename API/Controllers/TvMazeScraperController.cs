using Microsoft.AspNetCore.Mvc;
using TvMazeScraper.Services;

namespace TvMazeScraper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TvMazeScraperController : ControllerBase
    {
        private readonly TvMazeDataIngestionService _dataIngestionService;

        public TvMazeScraperController(TvMazeDataIngestionService dataIngestionService)
        {
            _dataIngestionService = dataIngestionService;
        }

        [HttpPost("scrape")]
        public async Task<IActionResult> ScrapeTvMazeData()
        {
            await _dataIngestionService.FetchAndPersistShowsWithCastAsync();
            return Ok("Scraping completed.");
        }
    }
}
