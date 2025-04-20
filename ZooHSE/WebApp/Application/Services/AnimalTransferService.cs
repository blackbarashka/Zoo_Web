using System;
using System.Collections.Generic;
using ZooHSE.WebApi.Application.Interfaces;
using ZooHSE.WebApi.Domain.Entities;
using ZooHSE.WebApi.Domain.Events;

namespace ZooHSE.WebApi.Application.Services
{
    /// <summary>
    /// Сервис для организации перемещения животных между вольерами.
    /// </summary>
    public class AnimalTransferService
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly IEnclosureRepository _enclosureRepository;
        private readonly List<AnimalMovedEvent> _events = new List<AnimalMovedEvent>();
        /// <summary>
        /// Конструктор сервиса перемещения животных.
        /// </summary>
        /// <param name="animalRepository"></param>
        /// <param name="enclosureRepository"></param>
        public AnimalTransferService(IAnimalRepository animalRepository, IEnclosureRepository enclosureRepository)
        {
            _animalRepository = animalRepository;
            _enclosureRepository = enclosureRepository;
        }
        /// <summary>
        /// Перемещение животного в другой вольер.
        /// </summary>
        /// <param name="animalId"></param>
        /// <param name="targetEnclosureId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public AnimalMovedEvent TransferAnimal(int animalId, int targetEnclosureId)
        {
            var animal = _animalRepository.GetById(animalId);
            if (animal == null)
                throw new ArgumentException($"Животное с ID {animalId} не найдено");

            var targetEnclosure = _enclosureRepository.GetById(targetEnclosureId);
            if (targetEnclosure == null)
                throw new ArgumentException($"Вольер с ID {targetEnclosureId} не найден");

            // Проверка совместимости вольера с животным
            if (!targetEnclosure.CanAddAnimal(animal))
                throw new InvalidOperationException("Животное не может быть перемещено в этот вольер");

            // Если животное уже в вольере, удалим его оттуда
            if (animal.EnclosureId.HasValue)
            {
                var currentEnclosure = _enclosureRepository.GetById(animal.EnclosureId.Value);
                if (currentEnclosure != null)
                {
                    currentEnclosure.RemoveAnimal(animalId);
                    _enclosureRepository.Update(currentEnclosure);
                }
            }

            // Перемещение животного
            var moveEvent = animal.MoveToEnclosure(targetEnclosureId);
            targetEnclosure.AddAnimal(animalId);

            // Сохранение изменений
            _animalRepository.Update(animal);
            _enclosureRepository.Update(targetEnclosure);

            // Добавление события в журнал событий
            _events.Add(moveEvent);

            return moveEvent;
        }
    }
}
