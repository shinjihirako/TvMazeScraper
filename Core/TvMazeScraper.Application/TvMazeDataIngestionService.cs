using AutoMapper;
using TvMazeScraper.Domain.Entities;
using TvMazeScraper.Persistence.Interfaces;

namespace TvMazeScraper.Services
{
    public class TvMazeDataIngestionService
    {
        private readonly ITvMazeApiClientService _apiClient;
        private readonly IShowRepository _showRepository;
        private readonly IMapper _mapper;

        public TvMazeDataIngestionService(ITvMazeApiClientService apiClient, IShowRepository showRepository, IMapper mapper)
        {
            _apiClient = apiClient;
            _showRepository = showRepository;
            _mapper = mapper;
        }

        public async Task  FetchAndPersistShowsWithCastAsync()
        {
            var shows = await _apiClient.GetShowsAsync(0); // You can loop pages here

            foreach (var show in shows)
            {
                var cast = await _apiClient.GetCastByShowIdAsync(show.Id);
                   show.Cast = cast.OrderByDescending(c => c.Birthday).ToList();

                // Map DTO to entity
                var showEntity = _mapper.Map<Show>(show);
                await _showRepository.SaveShowWithCastAsync(showEntity);
            }
        }
    }

}
