using Zoo.Domain.ValueObjects;

namespace Zoo.Domain.Entities
{
    public class Enclosure
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Type { get; set; }
        public int Capacity { get; set; }
        public List<Guid> AnimalIds { get; set; } = new();
        
        public bool CanAddAnimal(Species species, List<Species> currentSpecies)
        {
            if (AnimalIds.Count >= Capacity)
                return false;
            if (!currentSpecies.Any())
                return true;
            return currentSpecies.All(s => s.Diet == species.Diet);
        }

        public void AddAnimal(Guid animalId) => AnimalIds.Add(animalId);
        public void RemoveAnimal(Guid animalId) => AnimalIds.Remove(animalId);
    }
}