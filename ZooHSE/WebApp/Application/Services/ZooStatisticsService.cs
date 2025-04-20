using System.Collections.Generic;
using System.Linq;
using ZooHSE.WebApi.Application.Interfaces;
using ZooHSE.WebApi.Domain.Entities;
using ZooHSE.WebApi.Domain.Enums;

namespace ZooHSE.WebApi.Application.Services
{
    public class ZooStatisticsService
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly IEnclosureRepository _enclosureRepository;

        public ZooStatisticsService(IAnimalRepository animalRepository, IEnclosureRepository enclosureRepository)
        {
            _animalRepository = animalRepository;
            _enclosureRepository = enclosureRepository;
        }
        /// <summary>
        /// Получение общего количества животных в зоопарке.
        /// </summary>
        /// <returns></returns>
        public int GetTotalAnimalCount()
        {
            return _animalRepository.GetAll().Count();
        }
        /// <summary>
        /// Получение количества животных по типу.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetAnimalCountByType(AnimalType type)
        {
            return _animalRepository.GetAll().Count(a => a.Type == type);
        }
        /// <summary>
        /// Получение общего количества вольеров в зоопарке.
        /// </summary>
        /// <returns></returns>
        public int GetTotalEnclosureCount()
        {
            return _enclosureRepository.GetAll().Count();
        }
        /// <summary>
        /// Получение количества доступных вольеров (свободных или частично заполненных).
        /// </summary>
        /// <returns></returns>
        public int GetAvailableEnclosuresCount()
        {
            return _enclosureRepository.GetAll().Count(e => e.CurrentAnimalCount < e.MaxCapacity);
        }
        /// <summary>
        /// Получение количества животных по типам вольеров.
        /// </summary>
        /// <returns></returns>
        public Dictionary<EnclosureType, int> GetAnimalCountByEnclosureType()
        {
            var result = new Dictionary<EnclosureType, int>();

            foreach (var enclosure in _enclosureRepository.GetAll())
            {
                if (result.ContainsKey(enclosure.Type))
                {
                    result[enclosure.Type] += enclosure.CurrentAnimalCount;
                }
                else
                {
                    result[enclosure.Type] = enclosure.CurrentAnimalCount;
                }
            }

            return result;
        }
        /// <summary>
        /// Получение процента здоровых животных в зоопарке.
        /// </summary>
        /// <returns></returns>
        public int GetHealthyAnimalPercentage()
        {
            var animals = _animalRepository.GetAll().ToList();
            if (!animals.Any())
                return 0;

            var healthyCount = animals.Count(a => a.IsHealthy);
            return (int)(((double)healthyCount / animals.Count) * 100);
        }
    }
}
