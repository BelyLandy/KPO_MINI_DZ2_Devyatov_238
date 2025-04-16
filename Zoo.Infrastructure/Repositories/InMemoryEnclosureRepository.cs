using Zoo.Application.Interfaces;
using Zoo.Domain.Entities;

namespace Zoo.Infrastructure.Repositories
{
    public class InMemoryEnclosureRepository : IEnclosureRepository
    {
        private readonly List<Enclosure> _enclosures = new();

        public void Add(Enclosure enclosure)
        {
            _enclosures.Add(enclosure);
        }

        public IEnumerable<Enclosure> GetAll() => _enclosures;

        public Enclosure GetById(Guid id) => _enclosures.SingleOrDefault(e => e.Id == id);

        public void Remove(Guid id)
        {
            var enclosure = GetById(id);
            if (enclosure != null)
                _enclosures.Remove(enclosure);
        }
    }
}