using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using ZooHSE.WebApi.Domain.Entities;
using ZooHSE.WebApi.Domain.Enums;
using ZooHSE.WebApi.Domain.ValueObjects;
using ZooHSE.WebApi.Presentation;

public class AnimalControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AnimalControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }
    /// <summary>
    /// ���� ��� �������� ��������� ���� ��������.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetAll_ReturnsOkResult()
    {
        // Act
        var response = await _client.GetAsync("/api/animals");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotNull(content);
    }

    /// <summary>
    /// ���� ��� �������� ��������� ��������� �� ��������������.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetById_ExistingId_ReturnsAnimal()
    {
        // Arrange - ������� ��������
        var animal = new Animal
        {
            Name = "���",
            Species = "Panthera leo",
            BirthDate = new DateTime(2020, 1, 1),
            Gender = "M",
            FavoriteFood = new Food("����", 10), 
            IsHealthy = true,
            Type = AnimalType.Predator
        };

        var createResponse = await _client.PostAsJsonAsync("/api/animals", animal);
        createResponse.EnsureSuccessStatusCode();
        var createdAnimal = await JsonDocument.ParseAsync(
            await createResponse.Content.ReadAsStreamAsync());
        var animalId = createdAnimal.RootElement.GetProperty("id").GetInt32();

        // Act
        var response = await _client.GetAsync($"/api/animals/{animalId}");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("���", content);
    }
    /// <summary>
    /// ���� ��� �������� ��������� ��������� �� ��������������� ��������������.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetById_NonExistingId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/animals/999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    /// <summary>
    /// ���� ��� �������� �������� ������ ���������.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Create_ValidAnimal_ReturnsCreated()
    {
        // Arrange
        var animal = new Animal
        {
            Name = "�����",
            Species = "Giraffa camelopardalis",
            BirthDate = new DateTime(2018, 6, 15),
            Gender = "F",
            FavoriteFood = new Food("������", 5), 
            IsHealthy = true,
            Type = AnimalType.Herbivore,
            KindnessLevel = 8
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/animals", animal);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
    /// <summary>
    /// ���� ��� �������� �������� ��������� � ������������� �������.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Update_ValidAnimal_ReturnsNoContent()
    {
        // Arrange - ������� ��������
        var animal = new Animal
        {
            Name = "����",
            Species = "Loxodonta africana",
            BirthDate = new DateTime(2015, 3, 10),
            Gender = "�",
            FavoriteFood = new Food("������", 15),
            IsHealthy = true,
            Type = AnimalType.Herbivore,
            KindnessLevel = 7
        };

        var createResponse = await _client.PostAsJsonAsync("/api/animals", animal);
        createResponse.EnsureSuccessStatusCode();
        var createdAnimal = await JsonDocument.ParseAsync(
            await createResponse.Content.ReadAsStreamAsync());
        var animalId = createdAnimal.RootElement.GetProperty("id").GetInt32();

        // ����������
        animal.Id = animalId;
        animal.Name = "���� �����";

        // Act
        var response = await _client.PutAsJsonAsync($"/api/animals/{animalId}", animal);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // ���������, ��� ������ ����������
        var getResponse = await _client.GetAsync($"/api/animals/{animalId}");
        var content = await getResponse.Content.ReadAsStringAsync();
        Assert.Contains("���� �����", content);
    }
    /// <summary>
    /// ���� ��� �������� ���������� ��������� � ������������� �������.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Delete_ExistingId_ReturnsNoContent()
    {
        // Arrange - ������� ��������
        var animal = new Animal
        {
            Name = "�������",
            Species = "Spheniscus demersus",
            BirthDate = new DateTime(2019, 8, 22),
            Gender = "M",
            FavoriteFood = new Food("����", 2),
            IsHealthy = true,
            Type = AnimalType.Bird
        };

        var createResponse = await _client.PostAsJsonAsync("/api/animals", animal);
        createResponse.EnsureSuccessStatusCode();
        var createdAnimal = await JsonDocument.ParseAsync(
            await createResponse.Content.ReadAsStreamAsync());
        var animalId = createdAnimal.RootElement.GetProperty("id").GetInt32();

        // Act
        var response = await _client.DeleteAsync($"/api/animals/{animalId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // ���������, ��� �������� �������
        var getResponse = await _client.GetAsync($"/api/animals/{animalId}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}
