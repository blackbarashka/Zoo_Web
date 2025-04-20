using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using ZooHSE.WebApi.Domain.Entities;
using ZooHSE.WebApi.Domain.Enums;
using ZooHSE.WebApi.Presentation;
/// <summary>
/// Тесты для контроллера EnclosureController.
/// </summary>
public class EnclosureControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public EnclosureControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }
    /// <summary>
    /// Тест для проверки получения всех вольеров.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetAll_ReturnsOkResult()
    {
        // Act
        var response = await _client.GetAsync("/api/enclosures");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotNull(content);
    }
    /// <summary>
    /// Тест для проверки получения вольера по идентификатору.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetById_ExistingId_ReturnsEnclosure()
    {
        // Arrange - создаем вольер
        var enclosure = new Enclosure
        {
            Type = EnclosureType.Predators,
            Size = 100,
            MaxCapacity = 5
        };

        var createResponse = await _client.PostAsJsonAsync("/api/enclosures", enclosure);
        createResponse.EnsureSuccessStatusCode();
        var createdEnclosure = await JsonDocument.ParseAsync(
            await createResponse.Content.ReadAsStreamAsync());
        var enclosureId = createdEnclosure.RootElement.GetProperty("id").GetInt32();

        // Act
        var response = await _client.GetAsync($"/api/enclosures/{enclosureId}");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("predators", content.ToLower());
    }
    /// <summary>
    /// Тест для проверки создания нового вольера.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Create_ValidEnclosure_ReturnsCreated()
    {
        // Arrange
        var enclosure = new Enclosure
        {
            Type = EnclosureType.Herbivores,
            Size = 150,
            MaxCapacity = 8
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/enclosures", enclosure);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
    /// <summary>
    /// Тест для проверки обновления существующего вольера.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Update_ValidEnclosure_ReturnsNoContent()
    {
        // Arrange - создаем вольер
        var enclosure = new Enclosure
        {
            Type = EnclosureType.Aviary,
            Size = 80,
            MaxCapacity = 10
        };

        var createResponse = await _client.PostAsJsonAsync("/api/enclosures", enclosure);
        createResponse.EnsureSuccessStatusCode();
        var createdEnclosure = await JsonDocument.ParseAsync(
            await createResponse.Content.ReadAsStreamAsync());
        var enclosureId = createdEnclosure.RootElement.GetProperty("id").GetInt32();

        // Обновление
        enclosure.Id = enclosureId;
        enclosure.MaxCapacity = 12;

        // Act
        var response = await _client.PutAsJsonAsync($"/api/enclosures/{enclosureId}", enclosure);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    /// <summary>
    /// Тест для проверки очистки вольера по идентификатору.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task CleanEnclosure_ExistingId_ReturnsOk()
    {
        // Arrange - создаем вольер
        var enclosure = new Enclosure
        {
            Type = EnclosureType.Aquarium,
            Size = 200,
            MaxCapacity = 15
        };

        var createResponse = await _client.PostAsJsonAsync("/api/enclosures", enclosure);
        createResponse.EnsureSuccessStatusCode();
        var createdEnclosure = await JsonDocument.ParseAsync(
            await createResponse.Content.ReadAsStreamAsync());
        var enclosureId = createdEnclosure.RootElement.GetProperty("id").GetInt32();

        // Act
        var response = await _client.PostAsync($"/api/enclosures/{enclosureId}/clean", null);

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("убран", content);
    }
    /// <summary>
    /// Тест для проверки удаления вольера по идентификатору.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Delete_ExistingId_ReturnsNoContent()
    {
        // Arrange - создаем вольер
        var enclosure = new Enclosure
        {
            Type = EnclosureType.Predators,
            Size = 120,
            MaxCapacity = 4
        };

        var createResponse = await _client.PostAsJsonAsync("/api/enclosures", enclosure);
        createResponse.EnsureSuccessStatusCode();
        var createdEnclosure = await JsonDocument.ParseAsync(
            await createResponse.Content.ReadAsStreamAsync());
        var enclosureId = createdEnclosure.RootElement.GetProperty("id").GetInt32();

        // Act
        var response = await _client.DeleteAsync($"/api/enclosures/{enclosureId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}
