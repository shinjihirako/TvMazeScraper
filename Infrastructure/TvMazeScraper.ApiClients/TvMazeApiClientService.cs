
using System.Net.Http.Json;
using TvMazeScraper.Application.DTO;

namespace TvMazeScraper.Services
{
    public class TvMazeApiClientService : ITvMazeApiClientService
    {
        private readonly HttpClient _httpClient;

        public TvMazeApiClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.tvmaze.com/");
        }

        public async Task<List<ShowDto>> GetShowsAsync(int page)
        {
            var response = await _httpClient.GetFromJsonAsync<List<ShowDto>>($"shows?page={page}");
            return response ?? new List<ShowDto>();
        }

        public async Task<List<ShowDto>> GetAllShowsAsync()
        {
            var allShows = new List<ShowDto>();
            int page = 0;
            while (true)
            {
                var shows = await GetShowsAsync(page);
                if (shows.Count == 0) break;

                allShows.AddRange(shows);
                page++;
            }
            return allShows;
        }

        public async Task<List<CastDto>> GetCastByShowIdAsync(int showId)
        {
            var response = await _httpClient.GetFromJsonAsync<List<CastWrapperDto>>($"shows/{showId}/cast");

            var castList = new List<CastDto>();
            if (response != null)
            {
                foreach (var item in response)
                {
                    if (item.Person != null)
                    {
                        castList.Add(new CastDto
                        {
                            Id = item.Person.Id,
                            Name = item.Person.Name,
                            Birthday = item.Person.Birthday
                        });
                    }
                }
            }
            return castList;
        }
    }
}
