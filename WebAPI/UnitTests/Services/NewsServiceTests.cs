using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BLL.DTOs;
using BLL.Services;
using DAL.Entities;
using DAL.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace UnitTests.Services;

[TestFixture]
public class NewsServiceTests
{
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private NewsService _newsService;

    [SetUp]
    public void Setup()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _newsService = new NewsService(_mockUnitOfWork.Object);
    }

    [Test]
    public async Task GetAll_ReturnsCollection()
    {
        // Arrange
        var mockNewsList = new List<News>();
        _mockUnitOfWork.Setup(u => u.NewsRepository.GetAll()).ReturnsAsync(mockNewsList);

        // Act
        var result = await _newsService.GetAll();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetById_WithValidId_ReturnsNewsDTO()
    {
        // Arrange
        var newsId = 1;
        var mockNews = new News { Id = newsId, Title = "Test News"};
        _mockUnitOfWork.Setup(u => u.NewsRepository.GetById(newsId)).ReturnsAsync(mockNews);

        // Act
        var result = await _newsService.GetById(newsId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(newsId);
        result.Title.Should().Be("Test News");
    }

    [Test]
    public void GetById_WithInvalidId_ThrowsException()
    {
        // Arrange
        var newsId = 1;
        _mockUnitOfWork.Setup(u => u.NewsRepository.GetById(newsId)).ReturnsAsync((News)null);

        // Act & Assert
        Assert.ThrowsAsync<NullReferenceException>(async () => await _newsService.GetById(newsId));
    }

    [Test]
    public async Task Add_WithValidEntity_AddsToRepository()
    {
        // Arrange
        var newsDto = new NewsDTO { Title = "Test News" };

        // Act
        await _newsService.Add(newsDto);

        // Assert
        _mockUnitOfWork.Verify(u => u.NewsRepository.Add(It.IsAny<News>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task Delete_WithValidId_DeletesFromRepository()
    {
        // Arrange
        var newsId = 1;
        var mockNews = new News { Id = newsId, Title = "Test News" };
        _mockUnitOfWork.Setup(u => u.NewsRepository.GetById(newsId)).ReturnsAsync(mockNews);

        // Act
        await _newsService.Delete(newsId);

        // Assert
        _mockUnitOfWork.Verify(u => u.NewsRepository.Delete(It.IsAny<News>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public void Delete_WithInvalidId_ThrowsException()
    {
        // Arrange
        var newsId = 1;
        _mockUnitOfWork.Setup(u => u.NewsRepository.GetById(newsId)).ReturnsAsync((News)null);

        // Act & Assert
        Assert.ThrowsAsync<NullReferenceException>(async () => await _newsService.Delete(newsId));
    }

    [Test]
    public async Task Update_WithValidIdAndEntity_UpdatesRepository()
    {
        // Arrange
        var newsId = 1;
        var newsDto = new NewsDTO { Id = newsId, Title = "Updated News" };
        var mockNews = new News { Id = newsId, Title = "Original News" };

        _mockUnitOfWork.Setup(u => u.NewsRepository.GetById(newsId)).ReturnsAsync(mockNews);

        // Act
        await _newsService.Update(newsId, newsDto);

        // Assert
        _mockUnitOfWork.Verify(u => u.NewsRepository.Update(It.IsAny<News>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public void Update_WithInvalidId_ThrowsException()
    {
        // Arrange
        var invalidNewsId = 999; // Assuming there's no news with this ID
        var newsDto = new NewsDTO { Id = invalidNewsId, Title = "Updated News" };

        // Act & Assert
        Assert.ThrowsAsync<NullReferenceException>(async () => await _newsService.Update(invalidNewsId, newsDto));
    }

    [Test]
    public async Task GetAllByRubricId_ReturnsCollection()
    {
        // Arrange
        var rubricId = 1;
        var mockNewsList = new List<News>();
        _mockUnitOfWork.Setup(u => u.NewsRepository.GetAllByRubricId(rubricId)).ReturnsAsync(mockNewsList);

        // Act
        var result = await _newsService.GetAllByRubricId(rubricId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetAllByTagId_ReturnsCollection()
    {
        // Arrange
        var tagId = 1;
        var mockNewsList = new List<News>();
        _mockUnitOfWork.Setup(u => u.NewsRepository.GetAllByTagId(tagId)).ReturnsAsync(mockNewsList);

        // Act
        var result = await _newsService.GetAllByTagId(tagId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetAllByAuthorId_ReturnsCollection()
    {
        // Arrange
        var authorId = 1;
        var mockNewsList = new List<News>();
        _mockUnitOfWork.Setup(u => u.NewsRepository.GetAllByAuthorId(authorId)).ReturnsAsync(mockNewsList);

        // Act
        var result = await _newsService.GetAllByAuthorId(authorId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
