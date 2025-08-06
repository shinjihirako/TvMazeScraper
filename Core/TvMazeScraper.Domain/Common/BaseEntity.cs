using System.Security.Principal;

namespace TvMazeScraper.Domain.Common
{
    public abstract class BaseEntity 
    {
        public Guid Id { get; set; }
    }
}
