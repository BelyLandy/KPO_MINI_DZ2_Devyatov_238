using Zoo.Domain.Entities;

namespace Zoo.Application.Interfaces
{
    public interface IAnimalRepository
    {
        Animal GetById(Guid id);
        IEnumerable<Animal> GetAll();
        void Add(Animal animal);
        void Remove(Guid id);
    }
}