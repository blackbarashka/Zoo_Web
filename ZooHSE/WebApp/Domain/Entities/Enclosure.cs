using System.Collections.Generic;
using ZooHSE.WebApi.Domain.Enums;

namespace ZooHSE.WebApi.Domain.Entities
{
    /// <summary>
    /// Класс, представляющий вольер в зоопарке.
    /// </summary>
    public class Enclosure
    {
        public int Id { get; set; }
        public EnclosureType Type { get; set; }
        public int Size { get; set; }
        public int CurrentAnimalCount { get; set; }
        public int MaxCapacity { get; set; }
        public List<int> AnimalIds { get; set; } = new List<int>();

        public Enclosure() { }

        public Enclosure(EnclosureType type, int size, int maxCapacity)
        {
            Type = type;
            Size = size;
            MaxCapacity = maxCapacity;
            CurrentAnimalCount = 0;
        }
        /// <summary>
        /// Проверка возможности добавления животного в вольер. Если вольер заполнен, то добавление невозможно. Если вольер не заполнен, то проверяется тип животного и тип вольера. Если типы не совпадают, то добавление невозможно.
        /// </summary>
        /// <param name="animal"></param>
        /// <returns></returns>
        public bool CanAddAnimal(Animal animal)
        {
            if (CurrentAnimalCount >= MaxCapacity)
                return false;

            // Проверка совместимости типа вольера и типа животного
            bool isCompatible = (Type == EnclosureType.Predators && animal.Type == AnimalType.Predator) ||
                                (Type == EnclosureType.Herbivores && animal.Type == AnimalType.Herbivore) ||
                                (Type == EnclosureType.Aquarium && animal.Type == AnimalType.Aquatic) ||
                                (Type == EnclosureType.Aviary && animal.Type == AnimalType.Bird);

            return isCompatible;
        }
        /// <summary>
        /// Добавление животного в вольер. Если вольер заполнен, то добавление невозможно. Если вольер не заполнен, то проверяется тип животного и тип вольера. Если типы не совпадают, то добавление невозможно.
        /// </summary>
        /// <param name="animalId"></param>
        /// <returns></returns>
        public bool AddAnimal(int animalId)
        {
            if (CurrentAnimalCount < MaxCapacity)
            {
                AnimalIds.Add(animalId);
                CurrentAnimalCount++;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Удаление животного из вольера. Если животное не найдено, то удаление невозможно.
        /// </summary>
        /// <param name="animalId"></param>
        /// <returns></returns>
        public bool RemoveAnimal(int animalId)
        {
            if (AnimalIds.Contains(animalId))
            {
                AnimalIds.Remove(animalId);
                CurrentAnimalCount--;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Уборка вольера.
        /// </summary>
        public void Clean()
        {
            Console.WriteLine($"Вольер {Id} убран");
        }
    }
}
