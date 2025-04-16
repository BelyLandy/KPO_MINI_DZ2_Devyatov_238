namespace Zoo.Domain.Events
{
    public class FeedingTimeEvent
    {
        public Guid AnimalId { get; }
        public DateTime FeedingTime { get; }

        public FeedingTimeEvent(Guid animalId, DateTime time)
        {
            AnimalId = animalId;
            FeedingTime = time;
        }
    }
}