using Zoo.Application.Interfaces;
using Zoo.Domain.Entities;

namespace Zoo.Infrastructure.Repositories
{
    public class InMemoryFeedingScheduleRepository : IFeedingScheduleRepository
    {
        private readonly List<FeedingSchedule> _schedules = new();

        public void Add(FeedingSchedule schedule)
        {
            _schedules.Add(schedule);
        }

        public IEnumerable<FeedingSchedule> GetAll() => _schedules;

        public FeedingSchedule GetById(Guid id) => _schedules.SingleOrDefault(s => s.Id == id);

        public void Remove(Guid id)
        {
            var schedule = GetById(id);
            if (schedule != null)
                _schedules.Remove(schedule);
        }

        public void Update(FeedingSchedule schedule)
        {
            
        }
    }
}