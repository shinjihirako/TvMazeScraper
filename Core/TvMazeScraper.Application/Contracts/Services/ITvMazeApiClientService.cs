using TvMazeScraper.Application.DTO;

namespace TvMazeScraper.Services
{
    public interface ITvMazeApiClientService
    {
        Task<List<ShowDto>> GetShowsAsync(int page);
        Task<List<CastDto>> GetCastByShowIdAsync(int showId);
    }
}