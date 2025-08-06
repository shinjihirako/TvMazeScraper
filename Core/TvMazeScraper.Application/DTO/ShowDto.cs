using System.Text.Json.Serialization;

namespace TvMazeScraper.Application.DTO
{
    public class ShowDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime? Premiered { get; set; }
        public List<CastDto> Cast { get; set; } = new List<CastDto>();
    }
}