namespace ZooHSE.WebApi.Application.DTOs
{
    /// <summary>
    /// DTO для расписания кормления животных.  
    /// </summary>
    public class FeedingScheduleDto
    {
        public int Id { get; set; }
        public int AnimalId { get; set; }
        public DateTime FeedingTime { get; set; }
        public string FoodType { get; set; }
        public bool IsCompleted { get; set; }
    }
}
