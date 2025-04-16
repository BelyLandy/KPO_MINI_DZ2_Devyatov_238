using Zoo.Application.Interfaces;
using Zoo.Domain.Entities;

namespace Zoo.Infrastructure.Repositories
{
    public class InMemoryAnimalRepository : IAnimalRepository
    {
        private readonly List<Animal> _animals = new();

        public void Add(Animal animal)
        {
            _animals.Add(animal);
        }

        public IEnumerable<Animal> GetAll() => _animals;

        public Animal GetById(Guid id) => _animals.SingleOrDefault(a => a.Id == id);

        public void Remove(Guid id)
        {
            var animal = GetById(id);
            if (animal != null)
                _animals.Remove(animal);
        }
    }
}