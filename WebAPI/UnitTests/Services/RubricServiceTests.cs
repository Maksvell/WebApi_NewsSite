using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTOs;
using BLL.Interfaces;
using BLL.Services;
using BLL.Validators;
using DAL.Entities;
using DAL.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace UnitTests.Services;

[TestFixture]
public class RubricServiceTests
{
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private Mock<IMapper> _mockMapper;
    private RubricService _rubricService;

    [SetUp]
    public void Setup()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _rubricService = new RubricService(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Test]
    public async Task GetAll_ReturnsCollection()
    {
        // Arrange
        var mockRubrics = new List<Rubric> { new Rubric { Id = 1, Name = "Test Rubric" } };
        _mockUnitOfWork.Setup(u => u.RubricRepository.GetAll()).ReturnsAsync(mockRubrics);
        _mockMapper.Setup(m => m.Map<IEnumerable<Rubric>, IEnumerable<RubricDTO>>(mockRubrics))
                   .Returns(new List<RubricDTO> { new RubricDTO { Id = 1, Name = "Test Rubric" } });

        // Act
        var result = await _rubricService.GetAll();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().Id.Should().Be(1);
        result.First().Name.Should().Be("Test Rubric");
    }

    [Test]
    public async Task GetById_WithValidId_ReturnsRubricDTO()
    {
        // Arrange
        var rubricId = 1;
        var mockRubric = new Rubric { Id = rubricId, Name = "Test Rubric" };
        _mockUnitOfWork.Setup(u => u.RubricRepository.GetById(rubricId)).ReturnsAsync(mockRubric);
        _mockMapper.Setup(m => m.Map<RubricDTO>(mockRubric)).Returns(new RubricDTO { Id = rubricId, Name = "Test Rubric" });

        // Act
        var result = await _rubricService.GetById(rubricId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(rubricId);
        result.Name.Should().Be("Test Rubric");
    }

    [Test]
    public void GetById_WithInvalidId_ThrowsException()
    {
        // Arrange
        var rubricId = 1;
        _mockUnitOfWork.Setup(u => u.RubricRepository.GetById(rubricId)).ReturnsAsync((Rubric)null);

        // Act & Assert
        Assert.ThrowsAsync<NullReferenceException>(async () => await _rubricService.GetById(rubricId));
    }

    [Test]
    public async Task Add_WithValidEntity_AddsToRepository()
    {
        // Arrange
        var rubricDto = new RubricDTO { Name = "Test Rubric" };
        _mockMapper.Setup(m => m.Map<Rubric>(rubricDto)).Returns(new Rubric { Name = "Test Rubric" });
        _mockUnitOfWork.Setup(u => u.RubricRepository.Add(It.IsAny<Rubric>()));
        _mockUnitOfWork.Setup(u => u.RubricRepository.GetAll()).ReturnsAsync(new List<Rubric>());

        // Act
        await _rubricService.Add(rubricDto);

        // Assert
        _mockUnitOfWork.Verify(u => u.RubricRepository.Add(It.IsAny<Rubric>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task Delete_WithValidId_DeletesFromRepository()
    {
        // Arrange
        var rubricId = 1;
        var mockRubric = new Rubric { Id = rubricId, Name = "Test Rubric" };
        _mockUnitOfWork.Setup(u => u.RubricRepository.GetById(rubricId)).ReturnsAsync(mockRubric);

        // Act
        await _rubricService.Delete(rubricId);

        // Assert
        _mockUnitOfWork.Verify(u => u.RubricRepository.Delete(It.IsAny<Rubric>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public void Delete_WithInvalidId_ThrowsException()
    {
        // Arrange
        var rubricId = 1;
        _mockUnitOfWork.Setup(u => u.RubricRepository.GetById(rubricId)).ReturnsAsync((Rubric)null);

        // Act & Assert
        Assert.ThrowsAsync<NullReferenceException>(async () => await _rubricService.Delete(rubricId));
    }

    [Test]
    public async Task Update_WithValidIdAndEntity_UpdatesRepository()
    {
        // Arrange
        var rubricId = 1;
        var rubricDto = new RubricDTO { Id = rubricId, Name = "Updated Rubric" };
        var mockRubric = new Rubric { Id = rubricId, Name = "Original Rubric" };

        _mockUnitOfWork.Setup(u => u.RubricRepository.GetById(rubricId)).ReturnsAsync(mockRubric);

        // Act
        await _rubricService.Update(rubricId, rubricDto);

        // Assert
        _mockUnitOfWork.Verify(u => u.RubricRepository.Update(It.IsAny<Rubric>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public void Update_WithInvalidId_ThrowsException()
    {
        // Arrange
        var invalidRubricId = 999; // Assuming there's no rubric with this ID
        var rubricDto = new RubricDTO { Id = invalidRubricId, Name = "Updated Rubric" };

        // Act & Assert
        Assert.ThrowsAsync<NullReferenceException>(async () => await _rubricService.Update(invalidRubricId, rubricDto));
    }

}
