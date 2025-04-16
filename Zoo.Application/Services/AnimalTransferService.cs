using Zoo.Application.Interfaces;
using Zoo.Domain.Entities;

namespace Zoo.Application.Services
{
    public class AnimalTransferService
    {
        private readonly IAnimalRepository _animalRepo;
        private readonly IEnclosureRepository _enclosureRepo;

        public AnimalTransferService(IAnimalRepository animalRepo, IEnclosureRepository enclosureRepo)
        {
            _animalRepo = animalRepo;
            _enclosureRepo = enclosureRepo;
        }

        public void MoveAnimal(Guid animalId, Guid newEnclosureId)
        {
            var animal = _animalRepo.GetById(animalId);
            if (animal == null)
                throw new InvalidOperationException("Животное не найдено");

            var newEnclosure = _enclosureRepo.GetById(newEnclosureId);
            if (newEnclosure == null)
                throw new InvalidOperationException("Вольер не найден");

            var currentSpecies = _animalRepo.GetAll()
                .Where(a => newEnclosure.AnimalIds.Contains(a.Id))
                .Select(a => a.Species)
                .ToList();

            if (!newEnclosure.CanAddAnimal(animal.Species, currentSpecies))
                throw new InvalidOperationException("Нельзя добавить животное в данный вольер (несовместимость или нет места)");
            
            if (animal.EnclosureId.HasValue)
            {
                var oldEnclosure = _enclosureRepo.GetById(animal.EnclosureId.Value);
                oldEnclosure?.RemoveAnimal(animalId);
            }

            newEnclosure.AddAnimal(animalId);
            animal.MoveToEnclosure(newEnclosureId);
        }
    }
}