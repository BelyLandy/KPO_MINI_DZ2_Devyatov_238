using Zoo.Application.Interfaces;
using Zoo.Domain.Entities;

namespace Zoo.Application.Services
{
    public class FeedingOrganizationService
    {
        private readonly IAnimalRepository _animalRepo;
        private readonly IFeedingScheduleRepository _scheduleRepo;

        public FeedingOrganizationService(IAnimalRepository animalRepo, IFeedingScheduleRepository scheduleRepo)
        {
            _animalRepo = animalRepo;
            _scheduleRepo = scheduleRepo;
        }

        public void AddFeedingSchedule(Guid animalId, DateTime time, string foodType)
        {
            var animal = _animalRepo.GetById(animalId);
            if (animal == null)
                throw new InvalidOperationException("Животное не найдено");

            var schedule = new FeedingSchedule
            {
                AnimalId = animalId,
                Time = time,
                FoodType = foodType
            };

            _scheduleRepo.Add(schedule);
        }

        public void PerformFeeding(Guid scheduleId)
        {
            var schedule = _scheduleRepo.GetById(scheduleId);
            if (schedule == null)
                throw new InvalidOperationException("Расписание кормления не найдено");

            schedule.MarkAsCompleted();
            _scheduleRepo.Update(schedule);
        }
    }
}