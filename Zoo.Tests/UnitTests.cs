using Zoo.Application.Services;
using Zoo.Domain.Entities;
using Zoo.Domain.ValueObjects;
using Zoo.Application.Interfaces;
using Zoo.Infrastructure.Repositories;
using Zoo.Application.DTOs;

namespace Zoo.Tests
{
    public class UnitTests
    {
        private IAnimalRepository CreateAnimalRepo() => new InMemoryAnimalRepository();
        private IEnclosureRepository CreateEnclosureRepo() => new InMemoryEnclosureRepository();
        private IFeedingScheduleRepository CreateFeedingScheduleRepo() => new InMemoryFeedingScheduleRepository();


        [Fact]
        public void AnimalTransferService_MoveAnimal_SuccessfulMove()
        {
            var animalRepo = CreateAnimalRepo();
            var enclosureRepo = CreateEnclosureRepo();
            var transferService = new AnimalTransferService(animalRepo, enclosureRepo);

            var speciesHerb = new Species("Zebra", DietType.Herbivore);
            var animal = new Animal
            {
                Id = Guid.NewGuid(),
                Name = "Marty",
                Species = speciesHerb
            };
            animalRepo.Add(animal);

            var enclosure1 = new Enclosure
            {
                Id = Guid.NewGuid(),
                Name = "Enclosure 1",
                Capacity = 2
            };
            var enclosure2 = new Enclosure
            {
                Id = Guid.NewGuid(),
                Name = "Enclosure 2",
                Capacity = 2
            };
            enclosureRepo.Add(enclosure1);
            enclosureRepo.Add(enclosure2);

            transferService.MoveAnimal(animal.Id, enclosure2.Id);

            var movedAnimal = animalRepo.GetById(animal.Id);
            Assert.Equal(enclosure2.Id, movedAnimal.EnclosureId);
            Assert.Contains(animal.Id, enclosure2.AnimalIds);
        }

        [Fact]
        public void AnimalTransferService_MoveAnimal_ThrowsWhenAnimalNotFound()
        {
            var animalRepo = CreateAnimalRepo();
            var enclosureRepo = CreateEnclosureRepo();
            var transferService = new AnimalTransferService(animalRepo, enclosureRepo);

            var enclosure = new Enclosure
            {
                Id = Guid.NewGuid(),
                Name = "Enclosure",
                Capacity = 1
            };
            enclosureRepo.Add(enclosure);

            Assert.Throws<InvalidOperationException>(() =>
                transferService.MoveAnimal(Guid.NewGuid(), enclosure.Id));
        }

        [Fact]
        public void AnimalTransferService_MoveAnimal_ThrowsWhenEnclosureNotFound()
        {
            var animalRepo = CreateAnimalRepo();
            var enclosureRepo = CreateEnclosureRepo();
            var transferService = new AnimalTransferService(animalRepo, enclosureRepo);

            var speciesHerb = new Species("Zebra", DietType.Herbivore);
            var animal = new Animal { Id = Guid.NewGuid(), Name = "Marty", Species = speciesHerb };
            animalRepo.Add(animal);

            Assert.Throws<InvalidOperationException>(() => transferService.MoveAnimal(animal.Id, Guid.NewGuid()));
        }

        [Fact]
        public void FeedingOrganizationService_AddFeedingSchedule_AddsSuccessfully()
        {
            var animalRepo = CreateAnimalRepo();
            var scheduleRepo = CreateFeedingScheduleRepo();
            var feedingService = new FeedingOrganizationService(animalRepo, scheduleRepo);

            var speciesHerb = new Species("Elephant", DietType.Herbivore);
            var animal = new Animal
            {
                Id = Guid.NewGuid(),
                Name = "Dumbo",
                Species = speciesHerb
            };
            animalRepo.Add(animal);

            var feedingTime = DateTime.UtcNow.AddHours(1);
            feedingService.AddFeedingSchedule(animal.Id, feedingTime, "Fruits");

            var schedules = scheduleRepo.GetAll().ToList();
            Assert.Single(schedules);
            Assert.Equal(animal.Id, schedules[0].AnimalId);
            Assert.Equal("Fruits", schedules[0].FoodType);
            Assert.True((schedules[0].Time - feedingTime).Duration() < TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void FeedingOrganizationService_AddFeedingSchedule_ThrowsWhenAnimalNotFound()
        {
            var animalRepo = CreateAnimalRepo();
            var scheduleRepo = CreateFeedingScheduleRepo();
            var feedingService = new FeedingOrganizationService(animalRepo, scheduleRepo);

            var feedingTime = DateTime.UtcNow.AddHours(1);
            Assert.Throws<InvalidOperationException>(() =>
                feedingService.AddFeedingSchedule(Guid.NewGuid(), feedingTime, "Fruits"));
        }

        [Fact]
        public void ZooStatisticsService_GetStatistics_ReturnsCorrectData()
        {
            var animalRepo = CreateAnimalRepo();
            var enclosureRepo = CreateEnclosureRepo();

            var enclosure1 = new Enclosure
            {
                Id = Guid.NewGuid(),
                Name = "Enclosure A",
                Capacity = 2
            };
            var enclosure2 = new Enclosure
            {
                Id = Guid.NewGuid(),
                Name = "Enclosure B",
                Capacity = 2
            };
            enclosureRepo.Add(enclosure1);
            enclosureRepo.Add(enclosure2);

            var speciesOmni = new Species("Bear", DietType.Omnivore);
            var animal1 = new Animal
            {
                Id = Guid.NewGuid(),
                Name = "Baloo",
                Species = speciesOmni,
                EnclosureId = enclosure1.Id
            };
            enclosure1.AddAnimal(animal1.Id);
            animalRepo.Add(animal1);

            var statsService = new ZooStatisticsService(animalRepo, enclosureRepo);

            ZooStatsDto stats = statsService.GetStatistics();

            Assert.Equal(1, stats.TotalAnimals);
            Assert.Equal(2, stats.TotalEnclosures);
            Assert.Equal(1, stats.FreeEnclosures);
        }

        [Fact]
        public void Animal_Treat_ShouldSetHealthStatusToHealthy()
        {
            var animal = new Animal
            {
                Id = Guid.NewGuid(),
                Name = "TestAnimal",
                HealthStatus = "Sick",
                Species = new Species("TestSpecies", DietType.Omnivore)
            };

            animal.Treat();

            Assert.Equal("Healthy", animal.HealthStatus);
        }

        [Fact]
        public void Animal_MoveToEnclosure_RaisesAnimalMovedEvent()
        {
            var animal = new Animal
            {
                Id = Guid.NewGuid(),
                Name = "TestAnimal",
                Species = new Species("TestSpecies", DietType.Carnivore)
            };
            var newEnclosureId = Guid.NewGuid();
            animal.DomainEvents.Clear();

            animal.MoveToEnclosure(newEnclosureId);

            Assert.Equal(newEnclosureId, animal.EnclosureId);
            Assert.Single(animal.DomainEvents);
            Assert.IsType<Zoo.Domain.Events.AnimalMovedEvent>(animal.DomainEvents.First());
        }

        [Fact]
        public void FeedingSchedule_MarkAsCompleted_RaisesFeedingTimeEvent()
        {
            var schedule = new FeedingSchedule
            {
                AnimalId = Guid.NewGuid(),
                Time = DateTime.UtcNow,
                FoodType = "TestFood"
            };
            schedule.DomainEvents.Clear();

            schedule.MarkAsCompleted();

            Assert.True(schedule.IsCompleted);
            Assert.Single(schedule.DomainEvents);
            Assert.IsType<Zoo.Domain.Events.FeedingTimeEvent>(schedule.DomainEvents.First());
        }

        [Fact]
        public void Enclosure_CanAddAnimal_ReturnsFalse_WhenCapacityExceeded()
        {
            var enclosure = new Enclosure
            {
                Id = Guid.NewGuid(),
                Name = "Small Enclosure",
                Capacity = 1
            };
            enclosure.AnimalIds.Add(Guid.NewGuid());

            bool canAdd = enclosure.CanAddAnimal(new Species("TestSpecies", DietType.Herbivore),
                new System.Collections.Generic.List<Species> { new Species("TestSpecies", DietType.Herbivore) });

            Assert.False(canAdd);
        }

        [Fact]
        public void InMemoryAnimalRepository_BasicOperations()
        {
            var repo = new InMemoryAnimalRepository();
            var animal = new Animal { Id = Guid.NewGuid(), Name = "Test", Species = new Species("Test", DietType.Omnivore) };
            repo.Add(animal);
            Assert.Equal(animal, repo.GetById(animal.Id));
            repo.Remove(animal.Id);
            Assert.Null(repo.GetById(animal.Id));
        }

        [Fact]
        public void InMemoryEnclosureRepository_BasicOperations()
        {
            var repo = new InMemoryEnclosureRepository();
            var enclosure = new Enclosure { Id = Guid.NewGuid(), Name = "TestEnclosure", Capacity = 2 };
            repo.Add(enclosure);
            Assert.Equal(enclosure, repo.GetById(enclosure.Id));
            repo.Remove(enclosure.Id);
            Assert.Null(repo.GetById(enclosure.Id));
        }

        [Fact]
        public void InMemoryFeedingScheduleRepository_BasicOperations()
        {
            var repo = new InMemoryFeedingScheduleRepository();
            var schedule = new FeedingSchedule { AnimalId = Guid.NewGuid(), Time = DateTime.UtcNow, FoodType = "TestFood" };
            repo.Add(schedule);
            Assert.Equal(schedule, repo.GetById(schedule.Id));
            repo.Remove(schedule.Id);
            Assert.Null(repo.GetById(schedule.Id));
        }
    }
}
