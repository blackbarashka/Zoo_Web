using System;
using System.ComponentModel.DataAnnotations;
using ZooHSE.WebApi.Domain.Enums;
using ZooHSE.WebApi.Domain.Events;
using ZooHSE.WebApi.Domain.ValueObjects;

namespace ZooHSE.WebApi.Domain.Entities
{
    /// <summary>
    /// Класс, представляющий животное в зоопарке.
    /// </summary>
    public class Animal
    {
        public int Id { get; set; }
        /// Вид животного (тигр, слон и т.д.)
        [Required]
        public string Species { get; set; }
        /// Кличка животного
        [Required]
        public string Name { get; set; }
        /// Дата рождения животного
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public Food FavoriteFood { get; set; }
        public bool IsHealthy { get; set; }
        /// Вольер, в котором находится животное
        public int? EnclosureId { get; set; }
        /// Тип животного (хищник, травоядное и т.д.)
        public AnimalType Type { get; set; }

        // Специфические свойства для разных животных
        public int? KindnessLevel { get; set; } // Для травоядных
        public int? EarLength { get; set; } // Для кроликов
        public int? IQ { get; set; } // Для обезьян
        public int? TailLength { get; set; } // Для тигров

        public Animal() { }

        public Animal(string name, string species, DateTime birthDate, string gender,
            Food favoriteFood, bool isHealthy, AnimalType type)
        {
            Name = name;
            Species = species;
            BirthDate = birthDate;
            Gender = gender;
            FavoriteFood = favoriteFood;
            IsHealthy = isHealthy;
            Type = type;
        }

        public void Feed()
        {
            Console.WriteLine($"Животное {Name} покормлено");
        }

        public void Heal()
        {
            if (!IsHealthy)
            {
                IsHealthy = true;
                Console.WriteLine($"Животное {Name} вылечено");
            }
        }
        /// Метод для проверки здоровья животного.
        public AnimalMovedEvent MoveToEnclosure(int newEnclosureId)
        {
            var oldEnclosureId = EnclosureId;
            EnclosureId = newEnclosureId;
            return new AnimalMovedEvent(Id, oldEnclosureId, newEnclosureId);
        }
    }
}
