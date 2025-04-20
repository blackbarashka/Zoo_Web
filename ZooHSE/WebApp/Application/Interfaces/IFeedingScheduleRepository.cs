using System.Collections.Generic;
using ZooHSE.WebApi.Domain.Entities;

namespace ZooHSE.WebApi.Application.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с графиками кормления.
    /// </summary>
    public interface IFeedingScheduleRepository
    {
        IEnumerable<FeedingSchedule> GetAll();
        FeedingSchedule GetById(int id);
        IEnumerable<FeedingSchedule> GetByAnimalId(int animalId);
        void Add(FeedingSchedule schedule);
        void Update(FeedingSchedule schedule);
        void Delete(int id);
    }
}
