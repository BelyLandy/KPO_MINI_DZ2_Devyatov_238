using Microsoft.AspNetCore.Mvc;
using Zoo.Application.Interfaces;
using Zoo.Application.Services;
using Zoo.Domain.Entities;

namespace Zoo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalsController : ControllerBase
    {
        private readonly IAnimalRepository _animalRepo;
        private readonly AnimalTransferService _transferService;

        public AnimalsController(IAnimalRepository animalRepo, AnimalTransferService transferService)
        {
            _animalRepo = animalRepo;
            _transferService = transferService;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_animalRepo.GetAll());

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var animal = _animalRepo.GetById(id);
            if (animal == null)
                return NotFound();
            return Ok(animal);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Animal animal)
        {
            animal.Id = Guid.NewGuid();
            _animalRepo.Add(animal);
            return CreatedAtAction(nameof(Get), new { id = animal.Id }, animal);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _animalRepo.Remove(id);
            return NoContent();
        }

        // Перемещение животного: POST api/animals/{id}/move?toEnclosure={enclosureId}
        [HttpPost("{id}/move")]
        public IActionResult Move(Guid id, [FromQuery] Guid toEnclosure)
        {
            try
            {
                _transferService.MoveAnimal(id, toEnclosure);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}