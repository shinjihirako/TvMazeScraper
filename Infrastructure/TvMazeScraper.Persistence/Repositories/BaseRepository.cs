namespace TvMazeScraper.Persistence.Repositories
{
    public class BaseRepository<T> where T : class
    {
        protected readonly TvMazeScraperDbContext _dbContext;

        public BaseRepository(TvMazeScraperDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext), "DbContext cannot be null");
        }

        public virtual async Task<T> GetByIdASync(Guid id)
        {
            return await _dbContext.Set<T>().FindAsync(id) ?? throw new KeyNotFoundException($"Entity with ID {id} not found.");
        }
    }
}
