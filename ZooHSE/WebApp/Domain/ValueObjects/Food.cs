namespace ZooHSE.WebApi.Domain.ValueObjects
{
    /// <summary>
    /// Класс, представляющий еду для животных. Value Object.
    /// </summary>
    public class Food
    {
        public string Name { get; set; }
        public int Quantity { get; set; }

        public Food(string name, int quantity)
        {
            Name = name;
            Quantity = quantity;
        }
    }
}
