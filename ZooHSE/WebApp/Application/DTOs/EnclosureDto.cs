namespace ZooHSE.WebApi.Application.DTOs
{
    /// <summary>
    /// DTO для вольера.
    /// </summary>
    public class EnclosureDto
{
    public int Id { get; set; }
    public string Type { get; set; }
    public int Size { get; set; }
    public int CurrentAnimalCount { get; set; }
    public int MaxCapacity { get; set; }
}
}
