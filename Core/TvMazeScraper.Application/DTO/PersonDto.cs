namespace TvMazeScraper.Application.DTO
{
    public class PersonDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateOnly? Birthday { get; set; }
    }
}