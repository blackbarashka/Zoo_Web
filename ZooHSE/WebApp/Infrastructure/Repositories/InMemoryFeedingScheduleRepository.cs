using System.Collections.Generic;
using System.Linq;
using ZooHSE.WebApi.Application.Interfaces;
using ZooHSE.WebApi.Domain.Entities;

namespace ZooHSE.WebApi.Infrastructure.Repositories
{
    public class InMemoryFeedingScheduleRepository : IFeedingScheduleRepository
    {
        private readonly List<FeedingSchedule> _schedules = new List<FeedingSchedule>();
        private int _nextId = 1;

        public IEnumerable<FeedingSchedule> GetAll()
        {
            return _schedules;
        }
        /// <summary>
        /// Получить расписание кормления по идентификатору.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FeedingSchedule GetById(int id)
        {
            return _schedules.FirstOrDefault(s => s.Id == id);
        }
        /// <summary>
        /// Получить расписание кормления для конкретного животного по его идентификатору.
        /// </summary>
        /// <param name="animalId"></param>
        /// <returns></returns>
        public IEnumerable<FeedingSchedule> GetByAnimalId(int animalId)
        {
            return _schedules.Where(s => s.AnimalId == animalId);
        }
        /// <summary>
        /// Добавить новое расписание кормления в репозиторий.
        /// </summary>
        /// <param name="schedule"></param>
        public void Add(FeedingSchedule schedule)
        {
            schedule.Id = _nextId++;
            _schedules.Add(schedule);
        }
        /// <summary>
        /// Обновить существующее расписание кормления в репозитории.
        /// </summary>
        /// <param name="schedule"></param>
        public void Update(FeedingSchedule schedule)
        {
            var index = _schedules.FindIndex(s => s.Id == schedule.Id);
            if (index >= 0)
            {
                _schedules[index] = schedule;
            }
        }
        /// <summary>
        /// Удалить расписание кормления из репозитория по идентификатору.
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var schedule = GetById(id);
            if (schedule != null)
            {
                _schedules.Remove(schedule);
            }
        }
    }
}
