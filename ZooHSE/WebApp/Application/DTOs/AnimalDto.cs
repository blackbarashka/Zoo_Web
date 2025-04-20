namespace ZooHSE.WebApi.Application.DTOs
{
    /// <summary>
    /// DTO для животного.
    /// </summary>
    public class AnimalDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string FavoriteFood { get; set; }
        public bool IsHealthy { get; set; }
        public int? EnclosureId { get; set; }
    }
}
