using System.Collections.Generic;
using ZooHSE.WebApi.Domain.Entities;

namespace ZooHSE.WebApi.Application.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с животными.
    /// </summary>
    public interface IAnimalRepository
    {
        IEnumerable<Animal> GetAll();
        Animal GetById(int id);
        void Add(Animal animal);
        void Update(Animal animal);
        void Delete(int id);
    }
}
