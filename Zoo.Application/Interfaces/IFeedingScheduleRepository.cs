using Zoo.Domain.Entities;

namespace Zoo.Application.Interfaces
{
    public interface IFeedingScheduleRepository
    {
        FeedingSchedule GetById(Guid id);
        IEnumerable<FeedingSchedule> GetAll();
        void Add(FeedingSchedule schedule);
        void Remove(Guid id);
        void Update(FeedingSchedule schedule);
    }
}