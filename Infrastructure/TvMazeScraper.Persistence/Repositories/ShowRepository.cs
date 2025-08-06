using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Domain.Entities;
using TvMazeScraper.Persistence.Interfaces;

namespace TvMazeScraper.Persistence.Repositories
{
    public class ShowRepository : BaseRepository<Show>, IShowRepository
    {
        public ShowRepository(TvMazeScraperDbContext dbContext) : base(dbContext)
        {}

        public async Task<Show?> GetByIdAsync(Guid id) //TODO tja echt nodig
        {
            return await _dbContext.Shows
                .Include(s => s.CastMembers)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Show>> GetAllAsync(int page, int pageSize)
        {
            return await _dbContext.Shows
           .Include(s => s.CastMembers.OrderByDescending(cm => cm.Birthday)) 
           .OrderBy(s => s.Name)
           .Skip((page - 1) * pageSize)
           .Take(pageSize)
           .ToListAsync();
        }

        public async Task AddAsync(Show show)
        {
            await _dbContext.Shows.AddAsync(show);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task SaveShowWithCastAsync(Show show)
        {
            var existingShow = await _dbContext.Shows
                .Include(s => s.CastMembers)
                .FirstOrDefaultAsync(s => s.Id == show.Id);

            if (existingShow != null)
            {
                // Update existing show details
                existingShow.Name = show.Name;
                existingShow.Premiered = show.Premiered;

                // Remove existing cast members not present anymore
                _dbContext.CastMembers.RemoveRange(existingShow.CastMembers);

                // Add updated cast list
                existingShow.CastMembers = show.CastMembers;
            }
            else
            {
                // If show does not exist, add new
                await _dbContext.Shows.AddAsync(show);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
