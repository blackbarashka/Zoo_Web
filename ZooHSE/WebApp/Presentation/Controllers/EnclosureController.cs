using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using ZooHSE.WebApi.Application.Interfaces;
using ZooHSE.WebApi.Domain.Entities;

namespace ZooHSE.WebApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/enclosures")]
    public class EnclosureController : ControllerBase
    {
        private readonly IEnclosureRepository _enclosureRepository;

        public EnclosureController(IEnclosureRepository enclosureRepository)
        {
            _enclosureRepository = enclosureRepository;
        }

        /// <summary>
        /// Получить список всех вольеров.
        /// </summary>
        /// <returns>Список вольеров.</returns>
        [HttpGet]
        public ActionResult<IEnumerable<Enclosure>> GetAll()
        {
            return Ok(_enclosureRepository.GetAll());
        }

        /// <summary>
        /// Получить информацию о конкретном вольере.
        /// </summary>
        /// <param name="id">Идентификатор вольера.</param>
        /// <returns>Данные вольера.</returns>
        [HttpGet("{id}")]
        public ActionResult<Enclosure> GetById(int id)
        {
            var enclosure = _enclosureRepository.GetById(id);
            if (enclosure == null)
                return NotFound();

            return Ok(enclosure);
        }

        /// <summary>
        /// Добавить новый вольер.
        /// </summary>
        /// <param name="enclosure">Данные вольера.</param>
        /// <returns>Созданный вольер.</returns>
        [HttpPost]
        public ActionResult<Enclosure> Create([FromBody] Enclosure enclosure)
        {
            if (enclosure == null)
                return BadRequest();

            _enclosureRepository.Add(enclosure);

            return CreatedAtAction(nameof(GetById), new { id = enclosure.Id }, enclosure);
        }


        /// <summary>
        /// Обновить данные вольера.
        /// </summary>
        /// <param name="id">Идентификатор вольера.</param>
        /// <param name="enclosure">Обновлённые данные вольера.</param>
        /// <returns>Результат операции.</returns>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Enclosure enclosure)
        {
            if (enclosure == null || id != enclosure.Id)
                return BadRequest();

            var existingEnclosure = _enclosureRepository.GetById(id);
            if (existingEnclosure == null)
                return NotFound();

            _enclosureRepository.Update(enclosure);

            return NoContent();
        }

        /// <summary>
        /// Удалить вольер.
        /// </summary>
        /// <param name="id">Идентификатор вольера.</param>
        /// <returns>Результат операции.</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var enclosure = _enclosureRepository.GetById(id);
            if (enclosure == null)
                return NotFound();

            _enclosureRepository.Delete(id);

            return NoContent();
        }

        /// <summary>
        /// Выполнить уборку в вольере.
        /// </summary>
        /// <param name="id">Идентификатор вольера.</param>
        /// <returns>Результат операции.</returns>
        [HttpPost("{id}/clean")]
        public IActionResult CleanEnclosure(int id)
        {
            var enclosure = _enclosureRepository.GetById(id);
            if (enclosure == null)
                return NotFound();

            enclosure.Clean();
            _enclosureRepository.Update(enclosure);

            return Ok(new { message = $"Вольер {id} успешно убран" });
        }
    }
}
