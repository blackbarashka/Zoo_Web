using System;
using System.Collections.Generic;
using System.Linq;
using ZooHSE.WebApi.Application.Interfaces;
using ZooHSE.WebApi.Domain.Entities;
using ZooHSE.WebApi.Domain.Events;

namespace ZooHSE.WebApi.Application.Services
{
    /// <summary>
    /// Сервис для организации кормления животных.
    /// </summary>
    public class FeedingOrganizationService
    {
        private readonly IFeedingScheduleRepository _feedingScheduleRepository;
        private readonly IAnimalRepository _animalRepository;
        private readonly List<FeedingTimeEvent> _events = new List<FeedingTimeEvent>();

        public FeedingOrganizationService(IFeedingScheduleRepository feedingScheduleRepository,
                                         IAnimalRepository animalRepository)
        {
            _feedingScheduleRepository = feedingScheduleRepository;
            _animalRepository = animalRepository;
        }
        /// <summary>
        /// Добавление нового расписания кормления для животного.
        /// </summary>
        /// <param name="animalId"></param>
        /// <param name="feedingTime"></param>
        /// <param name="foodType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public FeedingSchedule AddFeedingSchedule(int animalId, DateTime feedingTime, string foodType)
        {
            var animal = _animalRepository.GetById(animalId);
            if (animal == null)
                throw new ArgumentException($"Животное с ID {animalId} не найдено");

            var schedule = new FeedingSchedule(animalId, feedingTime, foodType);
            _feedingScheduleRepository.Add(schedule);

            return schedule;
        }
        /// <summary>
        /// Обновление времени кормления в расписании.
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <param name="newFeedingTime"></param>
        /// <exception cref="ArgumentException"></exception>
        public void UpdateFeedingSchedule(int scheduleId, DateTime newFeedingTime)
        {
            var schedule = _feedingScheduleRepository.GetById(scheduleId);
            if (schedule == null)
                throw new ArgumentException($"Расписание с ID {scheduleId} не найдено");

            schedule.UpdateSchedule(newFeedingTime);
            _feedingScheduleRepository.Update(schedule);
        }
        /// <summary>
        /// Удаление расписания кормления.
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <exception cref="ArgumentException"></exception>
        public void MarkFeedingAsCompleted(int scheduleId)
        {
            var schedule = _feedingScheduleRepository.GetById(scheduleId);
            if (schedule == null)
                throw new ArgumentException($"Расписание с ID {scheduleId} не найдено");

            schedule.MarkAsCompleted();
            _feedingScheduleRepository.Update(schedule);

            var animal = _animalRepository.GetById(schedule.AnimalId);
            if (animal != null)
            {
                animal.Feed();
                _animalRepository.Update(animal);
            }

            // Создание события кормления
            var feedingEvent = new FeedingTimeEvent(schedule.AnimalId, schedule.FoodType, DateTime.UtcNow);
            _events.Add(feedingEvent);
        }
        /// <summary>
        /// Получение расписания кормления для конкретного животного.
        /// </summary>
        /// <param name="animalId"></param>
        /// <returns></returns>
        public List<FeedingSchedule> GetFeedingSchedulesByAnimal(int animalId)
        {
            return _feedingScheduleRepository.GetByAnimalId(animalId).ToList();
        }
        /// <summary>
        /// Получение всех расписаний кормления в зоопарке.
        /// </summary>
        /// <returns></returns>
        public List<FeedingSchedule> GetAllFeedingSchedules()
        {
            return _feedingScheduleRepository.GetAll().ToList();
        }
    }
}
