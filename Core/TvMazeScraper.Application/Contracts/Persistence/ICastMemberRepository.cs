using TvMazeScraper.Domain.Entities;
namespace TvMazeScraper.Persistence.Interfaces
{
    public interface ICastMemberRepository
    {
        Task<List<CastMember>> GetByShowIdAsync(int showId);
        Task AddRangeAsync(List<CastMember> castMembers);
        Task SaveChangesAsync();
    }
}
