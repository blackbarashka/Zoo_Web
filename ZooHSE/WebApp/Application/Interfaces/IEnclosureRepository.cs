using System.Collections.Generic;
using ZooHSE.WebApi.Domain.Entities;

namespace ZooHSE.WebApi.Application.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с вольерами.
    /// </summary>
    public interface IEnclosureRepository
    {
        IEnumerable<Enclosure> GetAll();
        Enclosure GetById(int id);
        void Add(Enclosure enclosure);
        void Update(Enclosure enclosure);
        void Delete(int id);
    }
}
