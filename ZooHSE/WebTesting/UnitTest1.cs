using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using WebApp;
using Microsoft.AspNetCore.Mvc.Testing;

public class FeedingScheduleControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public FeedingScheduleControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

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

    [Fact]
    public async Task CreateFeedingSchedule_ReturnsCreated()
    {
        // Arrange
        var request = new
        {
            AnimalId = 1,
            FeedingTime = "2025-04-20T10:00:00",
            FoodType = "Grass"
        };
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/feeding-schedules", content);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
    }
}