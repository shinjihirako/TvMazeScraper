using AutoMapper;
using Moq;
using TvMazeScraper.Application.DTO;
using TvMazeScraper.Domain.Entities;
using TvMazeScraper.Persistence.Interfaces;
using TvMazeScraper.Services;

namespace TvMazeScraper.Tests.Services
{
    public class TvMazeDataIngestionServiceTests
    {
        private readonly Mock<ITvMazeApiClientService> _apiClientMock;
        private readonly Mock<IShowRepository> _showRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly TvMazeDataIngestionService _service;

        public TvMazeDataIngestionServiceTests()
        {
            _apiClientMock = new Mock<ITvMazeApiClientService>();
            _showRepositoryMock = new Mock<IShowRepository>();
            _mapperMock = new Mock<IMapper>();

            _service = new TvMazeDataIngestionService(_apiClientMock.Object, _showRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task FetchAndPersistShowsWithCastAsync_Should_OrderCastByBirthdayDesc_And_SaveShow()
        {
            // Arrange
            var shows = new List<ShowDto>
            {
                new ShowDto { Id = 1, Name = "Test Show" }
            };

            var cast = new List<CastDto>
            {
                new CastDto { Name = "Uchiha Madara", Birthday = new DateTime(1985, 5, 10) },
                new CastDto { Name = "Zaraki Kenpachi", Birthday = new DateTime(1990, 3, 15) },
                new CastDto { Name = "Nara Shikamaru", Birthday = new DateTime(1994, 6, 18) },
                new CastDto { Name = "Batou", Birthday = new DateTime(1979, 7, 29) },
            };

            _apiClientMock.Setup(x => x.GetShowsAsync(It.IsAny<int>())).ReturnsAsync(shows);
            _apiClientMock.Setup(x => x.GetCastByShowIdAsync(1)).ReturnsAsync(cast);

            _mapperMock.Setup(m => m.Map<Show>(It.IsAny<ShowDto>()))
                       .Returns<ShowDto>(dto => new Show
                       {
                           Id = Guid.NewGuid(),
                           Name = dto.Name,
                           CastMembers = dto.Cast.ConvertAll(c => new CastMember
                           {
                               Name = c.Name,
                               Birthday = c.Birthday
                           })
                       });

            // Act
            await _service.FetchAndPersistShowsWithCastAsync();

            // Assert
            _showRepositoryMock.Verify(x => x.SaveShowWithCastAsync(It.Is<Show>(s =>
                 s.CastMembers != null &&
                 s.CastMembers.Count == 4 &&
                 s.CastMembers.ElementAt(0).Name == "Nara Shikamaru" &&
                 s.CastMembers.ElementAt(1).Name == "Zaraki Kenpachi" &&
                 s.CastMembers.ElementAt(2).Name == "Uchiha Madara" &&
                 s.CastMembers.ElementAt(3).Name == "Batou"
             )), Times.Once);
        }
    }
}
