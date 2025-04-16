using Microsoft.AspNetCore.Mvc;
using Zoo.Application.Services;

namespace Zoo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly ZooStatisticsService _statsService;

        public StatisticsController(ZooStatisticsService statsService)
        {
            _statsService = statsService;
        }

        [HttpGet]
        public IActionResult GetStatistics()
        {
            var stats = _statsService.GetStatistics();
            return Ok(stats);
        }
    }
}