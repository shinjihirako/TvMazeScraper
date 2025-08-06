namespace TvMazeScraper.Application.DTO
{
    public class PersonDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime? Birthday { get; set; }
    }
}