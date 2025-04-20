using System.Collections.Generic;
using System.Linq;
using ZooHSE.WebApi.Application.Interfaces;
using ZooHSE.WebApi.Domain.Entities;

namespace ZooHSE.WebApi.Infrastructure.Repositories
{
    /// <summary>
    /// Репозиторий для хранения животных в памяти.
    /// </summary>
    public class InMemoryAnimalRepository : IAnimalRepository
    {
        private readonly List<Animal> _animals = new List<Animal>();
        private int _nextId = 1;

        public IEnumerable<Animal> GetAll()
        {
            return _animals;
        }
        /// <summary>
        /// Получить животное по идентификатору.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Animal GetById(int id)
        {
            return _animals.FirstOrDefault(a => a.Id == id);
        }
        /// <summary>
        /// Добавить новое животное в репозиторий.
        /// </summary>
        /// <param name="animal"></param>
        public void Add(Animal animal)
        {
            animal.Id = _nextId++;
            _animals.Add(animal);
        }
        /// <summary>
        /// Обновить существующее животное в репозитории.
        /// </summary>
        /// <param name="animal"></param>
        public void Update(Animal animal)
        {
            var index = _animals.FindIndex(a => a.Id == animal.Id);
            if (index >= 0)
            {
                _animals[index] = animal;
            }
        }
        /// <summary>
        /// Удалить животное из репозитория по идентификатору.
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var animal = GetById(id);
            if (animal != null)
            {
                _animals.Remove(animal);
            }
        }
    }
}
