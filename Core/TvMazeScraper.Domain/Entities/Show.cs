using TvMazeScraper.Domain.Common;

namespace TvMazeScraper.Domain.Entities
{
    public class Show : BaseEntity
    {
        public int ExternalShowId { get; set; }  
        public required string Name { get; set; }
        public DateOnly? Premiered { get; set; }
        public ICollection<CastMember> CastMembers { get; set; } = new List<CastMember>();
    }
}
