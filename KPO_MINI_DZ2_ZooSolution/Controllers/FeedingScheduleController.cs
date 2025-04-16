using Microsoft.AspNetCore.Mvc;
using System;
using Zoo.Application.Interfaces;
using Zoo.Application.Services;
using Zoo.Domain.Entities;

namespace Zoo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedingScheduleController : ControllerBase
    {
        private readonly IFeedingScheduleRepository _scheduleRepo;
        private readonly FeedingOrganizationService _feedingService;

        public FeedingScheduleController(IFeedingScheduleRepository scheduleRepo, FeedingOrganizationService feedingService)
        {
            _scheduleRepo = scheduleRepo;
            _feedingService = feedingService;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_scheduleRepo.GetAll());

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var schedule = _scheduleRepo.GetById(id);
            if (schedule == null)
                return NotFound();
            return Ok(schedule);
        }

        [HttpPost]
        public IActionResult Create([FromQuery] Guid animalId, [FromQuery] DateTime time, [FromQuery] string foodType)
        {
            try
            {
                _feedingService.AddFeedingSchedule(animalId, time, foodType);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("{id}/execute")]
        public IActionResult Execute(Guid id)
        {
            try
            {
                _feedingService.PerformFeeding(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}