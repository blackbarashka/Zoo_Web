using System;

namespace ZooHSE.WebApi.Domain.Entities
{
    /// <summary>
    /// Класс, представляющий график кормления животных в зоопарке.
    /// </summary>
    public class FeedingSchedule
    {
        public int Id { get; set; }
        public int AnimalId { get; set; }
        public DateTime FeedingTime { get; set; }
        public string FoodType { get; set; }
        public bool IsCompleted { get; set; }

        public FeedingSchedule() { }

        public FeedingSchedule(int animalId, DateTime feedingTime, string foodType)
        {
            AnimalId = animalId;
            FeedingTime = feedingTime;
            FoodType = foodType;
            IsCompleted = false;
        }
        /// <summary>
        /// Обновление времени кормления в расписании.
        /// </summary>
        /// <param name="newFeedingTime"></param>
        public void UpdateSchedule(DateTime newFeedingTime)
        {
            FeedingTime = newFeedingTime;
        }
        /// <summary>
        /// Отметить кормление как выполненное.
        /// </summary>
        public void MarkAsCompleted()
        {
            IsCompleted = true;
        }
    }
}
