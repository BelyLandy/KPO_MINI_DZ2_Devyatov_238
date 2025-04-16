using Zoo.Domain.Events;

namespace Zoo.Domain.Entities
{
    public class FeedingSchedule
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AnimalId { get; set; }
        public DateTime Time { get; set; }
        public string FoodType { get; set; }
        public bool IsCompleted { get; private set; }

        public void MarkAsCompleted()
        {
            IsCompleted = true;
            DomainEvents.Add(new FeedingTimeEvent(AnimalId, Time));
        }

        public System.Collections.Generic.List<object> DomainEvents { get; } = new();
    }
}