using Zoo.Domain.Events;
using Zoo.Domain.ValueObjects;

namespace Zoo.Domain.Entities
{
    public class Animal
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Species Species { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string FavoriteFood { get; set; }
        public string HealthStatus { get; set; } = "Healthy";
        public Guid? EnclosureId { get; set; }
        
        public List<object> DomainEvents { get; } = new();

        public void Treat() => HealthStatus = "Healthy";

        public void MoveToEnclosure(Guid newEnclosureId)
        {
            var previousEnclosure = EnclosureId;
            EnclosureId = newEnclosureId;
            DomainEvents.Add(new AnimalMovedEvent(Id, previousEnclosure, newEnclosureId));
        }
    }
}