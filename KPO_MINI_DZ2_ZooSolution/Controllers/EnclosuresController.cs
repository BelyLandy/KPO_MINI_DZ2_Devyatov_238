using Microsoft.AspNetCore.Mvc;
using Zoo.Application.Interfaces;
using Zoo.Domain.Entities;

namespace Zoo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnclosuresController : ControllerBase
    {
        private readonly IEnclosureRepository _enclosureRepo;

        public EnclosuresController(IEnclosureRepository enclosureRepo)
        {
            _enclosureRepo = enclosureRepo;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_enclosureRepo.GetAll());

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var enclosure = _enclosureRepo.GetById(id);
            if (enclosure == null)
                return NotFound();
            return Ok(enclosure);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Enclosure enclosure)
        {
            enclosure.Id = Guid.NewGuid();
            _enclosureRepo.Add(enclosure);
            return CreatedAtAction(nameof(Get), new { id = enclosure.Id }, enclosure);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _enclosureRepo.Remove(id);
            return NoContent();
        }
    }
}