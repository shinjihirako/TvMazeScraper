using TvMazeScraper.Domain.Common;

namespace TvMazeScraper.Domain.Entities
{
    public class CastMember : BaseEntity
    {
        public int ExternalPersonId { get; set; }  
        public required string Name { get; set; }
        public DateTime? Birthday { get; set; }

        public Guid ShowId { get; set; }  
        public Show? Show { get; set; }
    }
}
