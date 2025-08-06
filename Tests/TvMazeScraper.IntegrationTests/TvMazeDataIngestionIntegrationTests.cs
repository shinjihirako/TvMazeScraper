using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Application.Profiles;
using TvMazeScraper.Persistence;
using TvMazeScraper.Persistence.Interfaces;
using TvMazeScraper.Persistence.Repositories;
using TvMazeScraper.Services;
using Xunit;

namespace TvMazeScraper.Tests.Integration
{
    public class TvMazeDataIngestionIntegrationTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly TvMazeScraperDbContext _dbContext;
        private readonly IShowRepository _showRepository;
        private readonly ITvMazeApiClientService _apiClientService;
        private readonly TvMazeDataIngestionService _service;

        public TvMazeDataIngestionIntegrationTests()
        {
            // SQLite In-Memory DB
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<TvMazeScraperDbContext>()
                .UseSqlite(_connection)
                .Options;

            _dbContext = new TvMazeScraperDbContext(options);
            _dbContext.Database.EnsureCreated();

            _showRepository = new ShowRepository(_dbContext);

            // Use the real implementation of TvMazeApiClientService (no mocks)
            var httpClient = new HttpClient();
            _apiClientService = new TvMazeApiClientService(httpClient); // <-- You must inject HttpClient in real code

            // Mapper can be AutoMapper or manual mapping
            var mapper = MapperConfig.Initialize(); // <-- Your actual mapper config here

            _service = new TvMazeDataIngestionService(_apiClientService, _showRepository, mapper);
        }

        [Fact]
        public async Task FetchAndPersistShowsWithCastAsync_Should_Persist_And_Order_CastMembers()
        {
            await EnsureApiIsReachable(); // <-- Connectivity check

            // Act
            await _service.FetchAndPersistShowsWithCastAsync();

            // Assert
            var show = await _dbContext.Shows.Include(s => s.CastMembers).FirstOrDefaultAsync();
            Assert.NotNull(show);
            Assert.True(show.CastMembers.Count > 0);

            var isOrdered = show.CastMembers
                .SequenceEqual(show.CastMembers.OrderByDescending(c => c.Birthday));

            Assert.True(isOrdered, "CastMembers should be ordered by Birthday descending");
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            _connection.Dispose();
        }

        private async Task EnsureApiIsReachable()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync("https://api.tvmaze.com/shows/1");

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"TVMaze API is not reachable. Status Code: {response.StatusCode}");
            }
        }
    }
}
