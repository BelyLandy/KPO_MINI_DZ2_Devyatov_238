namespace Zoo.Domain.Events
{
    public class AnimalMovedEvent
    {
        public Guid AnimalId { get; }
        public Guid? FromEnclosureId { get; }
        public Guid ToEnclosureId { get; }

        public AnimalMovedEvent(Guid animalId, Guid? from, Guid to)
        {
            AnimalId = animalId;
            FromEnclosureId = from;
            ToEnclosureId = to;
        }
    }
}