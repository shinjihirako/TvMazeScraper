using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TvMazeScraper.Application.DTO;
using TvMazeScraper.Persistence.Interfaces;

namespace TvMazeScraper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowsController : ControllerBase
    {
        private readonly IShowRepository _showRepository;
        private readonly IMapper _mapper;
        public ShowsController(IShowRepository showRepository, IMapper mapper)
        {
            _showRepository = showRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetShows([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var shows = await _showRepository.GetAllAsync(page, pageSize);
            var showDtos = _mapper.Map<List<ShowDto>>(shows);
            return Ok(showDtos);
        }
    }
}
