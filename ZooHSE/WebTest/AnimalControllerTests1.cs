using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using ZooHSE.WebApi.Application.Interfaces;
using ZooHSE.WebApi.Application.Services;
using ZooHSE.WebApi.Domain.Entities;
using ZooHSE.WebApi.Domain.Enums;
using ZooHSE.WebApi.Presentation.Controllers;

public class AnimalControllerTests1
{
    private readonly Mock<IAnimalRepository> _mockAnimalRepository;
    private readonly Mock<IEnclosureRepository> _mockEnclosureRepository;
    private readonly AnimalTransferService _animalTransferService;
    private readonly AnimalController _controller;

    public AnimalControllerTests1()
    {
        _mockAnimalRepository = new Mock<IAnimalRepository>();
        _mockEnclosureRepository = new Mock<IEnclosureRepository>();
        _animalTransferService = new AnimalTransferService(_mockAnimalRepository.Object, _mockEnclosureRepository.Object);
        _controller = new AnimalController(_mockAnimalRepository.Object, _animalTransferService);
    }

    [Fact]
    public void GetAll_ReturnsOkResult_WithListOfAnimals()
    {
        // Arrange
        var animals = new List<Animal>
    {
        new Animal { Id = 1, Name = "Лев", Type = AnimalType.Predator },
        new Animal { Id = 2, Name = "Слон", Type = AnimalType.Herbivore }
    };
        _mockAnimalRepository.Setup(repo => repo.GetAll()).Returns(animals);

        // Act
        var result = _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedAnimals = Assert.IsType<List<Animal>>(okResult.Value);
        Assert.Equal(2, returnedAnimals.Count);
    }

    [Fact]
    public void GetById_ExistingId_ReturnsAnimal()
    {
        // Arrange
        var animal = new Animal { Id = 1, Name = "Лев", Type = AnimalType.Predator };
        _mockAnimalRepository.Setup(repo => repo.GetById(1)).Returns(animal);

        // Act
        var result = _controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedAnimal = Assert.IsType<Animal>(okResult.Value);
        Assert.Equal("Лев", returnedAnimal.Name);
    }

    [Fact]
    public void GetById_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        _mockAnimalRepository.Setup(repo => repo.GetById(999)).Returns((Animal)null);

        // Act
        var result = _controller.GetById(999);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void Update_ValidAnimal_ReturnsNoContent()
    {
        // Arrange
        var animal = new Animal { Id = 1, Name = "Лев", Type = AnimalType.Predator };
        _mockAnimalRepository.Setup(repo => repo.GetById(1)).Returns(animal);
        _mockAnimalRepository.Setup(repo => repo.Update(It.IsAny<Animal>()));

        // Act
        var result = _controller.Update(1, animal);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void Update_NonExistingAnimal_ReturnsNotFound()
    {
        // Arrange
        _mockAnimalRepository.Setup(repo => repo.GetById(999)).Returns((Animal)null);

        // Act
        var result = _controller.Update(999, new Animal { Id = 999 });

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void TransferAnimal_ValidIds_ReturnsOk()
    {
        // Arrange
        var animal = new Animal { Id = 1, EnclosureId = 1 };
        var enclosure = new Enclosure { Id = 2, MaxCapacity = 5, CurrentAnimalCount = 0 };

        _mockAnimalRepository.Setup(repo => repo.GetById(1)).Returns(animal);
        _mockEnclosureRepository.Setup(repo => repo.GetById(2)).Returns(enclosure);
        _mockAnimalRepository.Setup(repo => repo.Update(It.IsAny<Animal>()));

        // Act
        var result = _controller.TransferAnimal(1, 2);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Contains("успешно перемещено", okResult.Value.ToString());
    }

    [Fact]
    public void TransferAnimal_InvalidAnimalId_ReturnsBadRequest()
    {
        // Arrange
        _mockAnimalRepository.Setup(repo => repo.GetById(999)).Returns((Animal)null);

        // Act
        var result = _controller.TransferAnimal(999, 2);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }


}
