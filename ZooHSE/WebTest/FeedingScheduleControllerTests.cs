using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using ZooHSE.WebApi.Domain.Entities;
using ZooHSE.WebApi.Presentation;
using ZooHSE.WebApi.Presentation.Controllers;

public class FeedingScheduleControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public FeedingScheduleControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }
    /// <summary>
    /// Тест для получения всех расписаний кормления.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetAllFeedingSchedules_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/feeding-schedules");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotNull(content);
    }
    /// <summary>
    /// Тест для создания нового расписания кормления.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task CreateFeedingSchedule_ReturnsCreated()
    {
        // Arrange
        var request = new FeedingScheduleRequest
        {
            AnimalId = 1,
            FeedingTime = DateTime.Now.AddHours(2),
            FoodType = "Мясо"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/feeding-schedules", request);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
    /// <summary>
    /// Тест для получения расписания кормления для конкретного животного.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetByAnimal_ReturnsSchedulesForAnimal()
    {
        // Arrange - создаем расписание для животного с ID 2
        var request = new FeedingScheduleRequest
        {
            AnimalId = 2,
            FeedingTime = DateTime.Now.AddHours(3),
            FoodType = "Трава"
        };
        await _client.PostAsJsonAsync("/api/feeding-schedules", request);

        // Act
        var response = await _client.GetAsync("/api/feeding-schedules/animal/2");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("\"animalId\":2", content.ToLower());
    }
    /// <summary>
    /// Тест для обновления времени кормления в расписании.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task UpdateFeedingTime_ValidId_ReturnsNoContent()
    {
        // Arrange - создаем расписание
        var request = new FeedingScheduleRequest
        {
            AnimalId = 3,
            FeedingTime = DateTime.Now.AddHours(4),
            FoodType = "Фрукты"
        };
        var createResponse = await _client.PostAsJsonAsync("/api/feeding-schedules", request);
        createResponse.EnsureSuccessStatusCode();
        var createdSchedule = await JsonDocument.ParseAsync(
            await createResponse.Content.ReadAsStreamAsync());
        var scheduleId = createdSchedule.RootElement.GetProperty("id").GetInt32();

        // Новое время кормления
        var newFeedingTime = DateTime.Now.AddHours(6);
        var content = new StringContent(
            JsonSerializer.Serialize(newFeedingTime),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PutAsync($"/api/feeding-schedules/{scheduleId}/update-time", content);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    /// <summary>
    /// Тест для удаления расписания кормления.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task MarkAsCompleted_ValidId_ReturnsOk()
    {
        // Arrange - создаем расписание
        var request = new FeedingScheduleRequest
        {
            AnimalId = 4,
            FeedingTime = DateTime.Now.AddHours(1),
            FoodType = "Рыба"
        };
        var createResponse = await _client.PostAsJsonAsync("/api/feeding-schedules", request);
        createResponse.EnsureSuccessStatusCode();
        var createdSchedule = await JsonDocument.ParseAsync(
            await createResponse.Content.ReadAsStreamAsync());
        var scheduleId = createdSchedule.RootElement.GetProperty("id").GetInt32();

        // Act
        var response = await _client.PutAsync($"/api/feeding-schedules/{scheduleId}/mark-completed", null);

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("выполненное", content);
    }
    /// <summary>
    /// Тест для обновления времени кормления с невалидным ID.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task UpdateFeedingTime_InvalidId_ReturnsBadRequest()
    {
        // Arrange
        var newFeedingTime = DateTime.Now.AddHours(6);
        var content = new StringContent(
            JsonSerializer.Serialize(newFeedingTime),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PutAsync("/api/feeding-schedules/999/update-time", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
