using TvMazeScraper.Domain.Entities;
namespace TvMazeScraper.Persistence.Interfaces
{
    public interface IShowRepository
    {
        Task<List<Show>> GetAllAsync(int page, int pageSize);
        Task AddAsync(Show show);
        Task SaveChangesAsync(); 
        Task SaveShowWithCastAsync(Show show);
    }
}
