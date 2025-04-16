using System.Linq;
using Zoo.Application.DTOs;
using Zoo.Application.Interfaces;

namespace Zoo.Application.Services
{
    public class ZooStatisticsService
    {
        private readonly IAnimalRepository _animalRepo;
        private readonly IEnclosureRepository _enclosureRepo;

        public ZooStatisticsService(IAnimalRepository animalRepo, IEnclosureRepository enclosureRepo)
        {
            _animalRepo = animalRepo;
            _enclosureRepo = enclosureRepo;
        }

        public ZooStatsDto GetStatistics()
        {
            var animals = _animalRepo.GetAll();
            var enclosures = _enclosureRepo.GetAll();
            return new ZooStatsDto
            {
                TotalAnimals = animals.Count(),
                TotalEnclosures = enclosures.Count(),
                FreeEnclosures = enclosures.Count(e => e.AnimalIds.Count == 0)
            };
        }
    }
}
