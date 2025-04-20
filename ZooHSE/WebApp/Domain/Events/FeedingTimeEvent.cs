using System;

namespace ZooHSE.WebApi.Domain.Events
{
    /// <summary>
    /// Событие, представляющее время кормления животного.
    /// </summary>
    public class FeedingTimeEvent
    {
        public int AnimalId { get; }
        public string FoodType { get; }
        public DateTime FeedingTime { get; }

        public FeedingTimeEvent(int animalId, string foodType, DateTime feedingTime)
        {
            AnimalId = animalId;
            FoodType = foodType;
            FeedingTime = feedingTime;
        }
    }
}
