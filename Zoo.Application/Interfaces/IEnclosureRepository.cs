using Zoo.Domain.Entities;

namespace Zoo.Application.Interfaces
{
    public interface IEnclosureRepository
    {
        Enclosure GetById(Guid id);
        IEnumerable<Enclosure> GetAll();
        void Add(Enclosure enclosure);
        void Remove(Guid id);
    }
}