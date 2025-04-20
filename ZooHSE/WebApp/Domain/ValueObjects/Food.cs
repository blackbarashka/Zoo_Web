namespace ZooHSE.WebApi.Domain.ValueObjects
{
    /// <summary>
    /// Класс, представляющий еду для животных. Value Object.
    /// </summary>
    public class Food
    {
        public string Name { get; }
        public int Quantity { get; }

        public Food(string name, int quantity)
        {
            Name = name;
            Quantity = quantity;
        }
    }
}
