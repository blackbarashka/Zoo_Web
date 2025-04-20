using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ZooHSE.WebApi.Application.Services;
using ZooHSE.WebApi.Domain.Enums;

namespace ZooHSE.WebApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/statistics")]
    public class StatisticsController : ControllerBase
    {
        private readonly ZooStatisticsService _statisticsService;

        public StatisticsController(ZooStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        /// <summary>
        /// Получить общее количество животных в зоопарке.
        /// </summary>
        /// <returns>Общее количество животных.</returns>
        [HttpGet("animals/count")]
        public ActionResult<int> GetTotalAnimalCount()
        {
            return Ok(_statisticsService.GetTotalAnimalCount());
        }

        /// <summary>
        /// Получить количество животных по типам.
        /// </summary>
        /// <returns>Словарь с количеством животных каждого типа.</returns>
        [HttpGet("animals/count/type")]
        public ActionResult<Dictionary<string, int>> GetAnimalCountByType()
        {
            var result = new Dictionary<string, int>();
            result.Add("Predator", _statisticsService.GetAnimalCountByType(AnimalType.Predator));
            result.Add("Herbivore", _statisticsService.GetAnimalCountByType(AnimalType.Herbivore));
            result.Add("Aquatic", _statisticsService.GetAnimalCountByType(AnimalType.Aquatic));
            result.Add("Bird", _statisticsService.GetAnimalCountByType(AnimalType.Bird));

            return Ok(result);
        }

        /// <summary>
        /// Получить общее количество вольеров в зоопарке.
        /// </summary>
        /// <returns>Общее количество вольеров.</returns>
        [HttpGet("enclosures/count")]
        public ActionResult<int> GetTotalEnclosureCount()
        {
            return Ok(_statisticsService.GetTotalEnclosureCount());
        }

        /// <summary>
        /// Получить количество доступных вольеров.
        /// </summary>
        /// <returns>Количество свободных или частично заполненных вольеров.</returns>
        [HttpGet("enclosures/count/available")]
        public ActionResult<int> GetAvailableEnclosuresCount()
        {
            return Ok(_statisticsService.GetAvailableEnclosuresCount());
        }

        /// <summary>
        /// Получить процент здоровых животных.
        /// </summary>
        /// <returns>Процент здоровых животных от общего числа.</returns>
        [HttpGet("animals/health-percentage")]
        public ActionResult<int> GetHealthyAnimalPercentage()
        {
            return Ok(_statisticsService.GetHealthyAnimalPercentage());
        }
    }
}
