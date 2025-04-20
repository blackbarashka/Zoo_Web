using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ZooHSE.WebApi.Application.Interfaces;
using ZooHSE.WebApi.Application.Services;
using ZooHSE.WebApi.Domain.Entities;
using ZooHSE.WebApi.Domain.Enums;
using ZooHSE.WebApi.Domain.ValueObjects;

namespace ZooHSE.WebApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/animals")]
    public class AnimalController : ControllerBase
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly AnimalTransferService _animalTransferService;

        public AnimalController(IAnimalRepository animalRepository, AnimalTransferService animalTransferService)
        {
            _animalRepository = animalRepository;
            _animalTransferService = animalTransferService;
        }
        /// <summary>
        /// Получить список всех животных.
        /// </summary>
        /// <returns>Список животных.</returns>
        [HttpGet]
        public ActionResult<IEnumerable<Animal>> GetAll()
        {
            return Ok(_animalRepository.GetAll());
        }
        /// <summary>
        /// Получить информацию о конкретном животном.
        /// </summary>
        /// <param name="id">Идентификатор животного.</param>
        /// <returns>Данные животного.</returns>
        [HttpGet("{id}")]
        public ActionResult<Animal> GetById(int id)
        {
            var animal = _animalRepository.GetById(id);
            if (animal == null)
                return NotFound();

            return Ok(animal);
        }
        /// <summary>
        /// Добавить новое животное. type: 0 - хищник, 1 - травоядное, 2 - рыба, 3 - птица
        /// </summary>
        /// <param name="animalDto">Данные животного.</param>
        /// <returns>Результат операции.</returns>
        [HttpPost]
        public ActionResult<Animal> Create([FromBody] Animal animal)
        {
            if (animal == null)
                return BadRequest();

            _animalRepository.Add(animal);

            return CreatedAtAction(nameof(GetById), new { id = animal.Id }, animal);
        }

        /// <summary>
        /// Обновить данные животного.
        /// </summary>
        /// <param name="id">Идентификатор животного.</param>
        /// <param name="animalDto">Обновлённые данные животного.</param>
        /// <returns>Результат операции.</returns>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Animal animal)
        {
            if (animal == null || id != animal.Id)
                return BadRequest();

            var existingAnimal = _animalRepository.GetById(id);
            if (existingAnimal == null)
                return NotFound();

            _animalRepository.Update(animal);

            return NoContent();
        }
        /// <summary>
        /// Удалить животное.
        /// </summary>
        /// <param name="id">Идентификатор животного.</param>
        /// <returns>Результат операции.</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var animal = _animalRepository.GetById(id);
            if (animal == null)
                return NotFound();

            _animalRepository.Delete(id);

            return NoContent();
        }

        /// <summary>
        /// Переместить животное в другой вольер.
        /// </summary>
        /// <param name="animalId">Идентификатор животного.</param>
        /// <param name="enclosureId">Идентификатор целевого вольера.</param>
        /// <returns>Результат операции перемещения.</returns>
        [HttpPost("{animalId}/transfer/{enclosureId}")]
        public IActionResult TransferAnimal(int animalId, int enclosureId)
        {
            try
            {
                var moveEvent = _animalTransferService.TransferAnimal(animalId, enclosureId);
                return Ok(new { message = $"Животное успешно перемещено из вольера {moveEvent.OldEnclosureId} в вольер {moveEvent.NewEnclosureId}" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Вылечить больное животное.
        /// </summary>
        /// <param name="id">Идентификатор животного.</param>
        /// <returns>Результат операции лечения.</returns>
        [HttpPost("{id}/heal")]
        public IActionResult HealAnimal(int id)
        {
            var animal = _animalRepository.GetById(id);
            if (animal == null)
                return NotFound();

            animal.Heal();
            _animalRepository.Update(animal);

            return Ok(new { message = $"Животное {animal.Name} успешно вылечено" });
        }
    }
}
