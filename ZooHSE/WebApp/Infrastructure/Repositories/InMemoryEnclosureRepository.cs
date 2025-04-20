using System.Collections.Generic;
using System.Linq;
using ZooHSE.WebApi.Application.Interfaces;
using ZooHSE.WebApi.Domain.Entities;

namespace ZooHSE.WebApi.Infrastructure.Repositories
{
    public class InMemoryEnclosureRepository : IEnclosureRepository
    {
        private readonly List<Enclosure> _enclosures = new List<Enclosure>();
        private int _nextId = 1;

        public IEnumerable<Enclosure> GetAll()
        {
            return _enclosures;
        }
        /// <summary>
        /// Получить вольер по идентификатору.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Enclosure GetById(int id)
        {
            return _enclosures.FirstOrDefault(e => e.Id == id);
        }
        /// <summary>
        /// Добавить новый вольер в репозиторий.
        /// </summary>
        /// <param name="enclosure"></param>
        public void Add(Enclosure enclosure)
        {
            enclosure.Id = _nextId++;
            _enclosures.Add(enclosure);
        }
        /// <summary>
        /// Обновить существующий вольер в репозитории.
        /// </summary>
        /// <param name="enclosure"></param>
        public void Update(Enclosure enclosure)
        {
            var index = _enclosures.FindIndex(e => e.Id == enclosure.Id);
            if (index >= 0)
            {
                _enclosures[index] = enclosure;
            }
        }
        /// <summary>
        /// Удалить вольер из репозитория по идентификатору.
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var enclosure = GetById(id);
            if (enclosure != null)
            {
                _enclosures.Remove(enclosure);
            }
        }
    }
}
