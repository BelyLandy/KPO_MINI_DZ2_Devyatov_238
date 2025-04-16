namespace Zoo.Domain.ValueObjects
{
    public class Species
    {
        public string Name { get; }
        public DietType Diet { get; }

        public Species(string name, DietType diet)
        {
            Name = name;
            Diet = diet;
        }

        public override bool Equals(object obj)
        {
            if (obj is not Species other)
                return false;
            return Name == other.Name && Diet == other.Diet;
        }

        public override int GetHashCode() => HashCode.Combine(Name, Diet);
    }
}