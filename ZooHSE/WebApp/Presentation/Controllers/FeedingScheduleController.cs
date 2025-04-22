using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using ZooHSE.WebApi.Application.Interfaces;
using ZooHSE.WebApi.Application.Services;
using ZooHSE.WebApi.Domain.Entities;

namespace ZooHSE.WebApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/feeding-schedules")]
    public class FeedingScheduleController : ControllerBase
    {
        private readonly FeedingOrganizationService _feedingService;

        public FeedingScheduleController(FeedingOrganizationService feedingService)
        {
            _feedingService = feedingService;
        }

        /// <summary>
        /// Получить список всех расписаний кормления.
        /// </summary>
        /// <returns>Список расписаний кормления.</returns>
        [HttpGet]
        public ActionResult<IEnumerable<FeedingSchedule>> GetAll()
        {
            return Ok(_feedingService.GetAllFeedingSchedules());
        }

        /// <summary>
        /// Получить расписания кормления для конкретного животного.
        /// </summary>
        /// <param name="animalId">Идентификатор животного.</param>
        /// <returns>Список расписаний кормления для указанного животного.</returns>
        [HttpGet("animal/{animalId}")]
        public ActionResult<IEnumerable<FeedingSchedule>> GetByAnimal(int animalId)
        {
            return Ok(_feedingService.GetFeedingSchedulesByAnimal(animalId));
        }

        /// <summary>
        /// Создать новое расписание кормления.
        /// </summary>
        /// <param name="request">Данные для создания расписания кормления.</param>
        /// <returns>Созданное расписание кормления.</returns>
        [HttpPost]
        public ActionResult<FeedingSchedule> Create(


        [FromQuery] int animalId,
        [FromQuery] DateTime feedingTime,
        [FromQuery] string foodType)
        {
            try
            {
                var schedule = new FeedingSchedule
                {
                    AnimalId = animalId,
                    FeedingTime = feedingTime,
                    FoodType = foodType,
                    IsCompleted = false
                };

                var huent = _feedingService.AddFeedingSchedule(schedule.AnimalId, schedule.FeedingTime, schedule.FoodType);

                return CreatedAtAction(nameof(GetAll), new { id = huent.Id }, huent);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        /// <summary>
        /// Обновить время кормления в расписании.
        /// </summary>
        /// <param name="id">Идентификатор расписания.</param>
        /// <param name="newFeedingTime">Новое время кормления.</param>
        /// <returns>Результат операции.</returns>
        [HttpPut("{id}/update-time")]
        public IActionResult UpdateFeedingTime(int id, [FromBody] DateTime newFeedingTime)
        {
            try
            {
                _feedingService.UpdateFeedingSchedule(id, newFeedingTime);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Отметить кормление как выполненное.
        /// </summary>
        /// <param name="id">Идентификатор расписания кормления.</param>
        /// <returns>Результат операции.</returns>
        [HttpPut("{id}/mark-completed")]
        public IActionResult MarkAsCompleted(int id)
        {
            try
            {
                _feedingService.MarkFeedingAsCompleted(id);
                return Ok(new { message = "Кормление отмечено как выполненное" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
    /// <summary>
    /// Модель запроса для создания расписания кормления.
    /// </summary>
    public class FeedingScheduleRequest
    {
        public int AnimalId { get; set; }
        public DateTime FeedingTime { get; set; }
        public string FoodType { get; set; }
    }
}
