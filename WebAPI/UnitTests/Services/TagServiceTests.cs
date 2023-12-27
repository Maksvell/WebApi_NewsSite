using System;
using System.Collections.Generic;
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
public class TagServiceTests
{
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private Mock<IMapper> _mockMapper;
    private TagService _tagService;

    [SetUp]
    public void Setup()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _tagService = new TagService(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Test]
    public async Task GetAll_ReturnsCollection()
    {
        // Arrange
        var mockTags = new List<Tag> { new Tag { Id = 1, Name = "Test Tag" } };
        _mockUnitOfWork.Setup(u => u.TagRepository.GetAll()).ReturnsAsync(mockTags);
        _mockMapper.Setup(m => m.Map<IEnumerable<Tag>, IEnumerable<TagDTO>>(mockTags))
                   .Returns(new List<TagDTO> { new TagDTO { Id = 1, Name = "Test Tag" } });

        // Act
        var result = await _tagService.GetAll();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().Id.Should().Be(1);
        result.First().Name.Should().Be("Test Tag");
    }

    [Test]
    public async Task GetById_WithValidId_ReturnsTagDTO()
    {
        // Arrange
        var tagId = 1;
        var mockTag = new Tag { Id = tagId, Name = "Test Tag" };
        _mockUnitOfWork.Setup(u => u.TagRepository.GetById(tagId)).ReturnsAsync(mockTag);
        _mockMapper.Setup(m => m.Map<TagDTO>(mockTag)).Returns(new TagDTO { Id = tagId, Name = "Test Tag" });

        // Act
        var result = await _tagService.GetById(tagId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(tagId);
        result.Name.Should().Be("Test Tag");
    }

    [Test]
    public void GetById_WithInvalidId_ThrowsException()
    {
        // Arrange
        var tagId = 1;
        _mockUnitOfWork.Setup(u => u.TagRepository.GetById(tagId)).ReturnsAsync((Tag)null);

        // Act & Assert
        Assert.ThrowsAsync<NullReferenceException>(async () => await _tagService.GetById(tagId));
    }

    [Test]
    public async Task Add_WithValidEntity_AddsToRepository()
    {
        // Arrange
        var tagDto = new TagDTO { Name = "Test Tag" };
        _mockMapper.Setup(m => m.Map<Tag>(tagDto)).Returns(new Tag { Name = "Test Tag" });
        _mockUnitOfWork.Setup(u => u.TagRepository.Add(It.IsAny<Tag>()));
        _mockUnitOfWork.Setup(u => u.TagRepository.GetAll()).ReturnsAsync(new List<Tag>());

        // Act
        await _tagService.Add(tagDto);

        // Assert
        _mockUnitOfWork.Verify(u => u.TagRepository.Add(It.IsAny<Tag>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task Delete_WithValidId_DeletesFromRepository()
    {
        // Arrange
        var tagId = 1;
        var mockTag = new Tag { Id = tagId, Name = "Test Tag" };
        _mockUnitOfWork.Setup(u => u.TagRepository.GetById(tagId)).ReturnsAsync(mockTag);

        // Act
        await _tagService.Delete(tagId);

        // Assert
        _mockUnitOfWork.Verify(u => u.TagRepository.Delete(It.IsAny<Tag>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public void Delete_WithInvalidId_ThrowsException()
    {
        // Arrange
        var tagId = 1;
        _mockUnitOfWork.Setup(u => u.TagRepository.GetById(tagId)).ReturnsAsync((Tag)null);

        // Act & Assert
        Assert.ThrowsAsync<NullReferenceException>(async () => await _tagService.Delete(tagId));
    }

    [Test]
    public async Task Update_WithValidIdAndEntity_UpdatesRepository()
    {
        // Arrange
        var tagId = 1;
        var tagDto = new TagDTO { Id = tagId, Name = "Updated Tag" };
        var mockTag = new Tag { Id = tagId, Name = "Original Tag" };

        _mockUnitOfWork.Setup(u => u.TagRepository.GetById(tagId)).ReturnsAsync(mockTag);

        // Act
        await _tagService.Update(tagId, tagDto);

        // Assert
        _mockUnitOfWork.Verify(u => u.TagRepository.Update(It.IsAny<Tag>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public void Update_WithInvalidId_ThrowsException()
    {
        // Arrange
        var invalidTagId = 999; // Assuming there's no tag with this ID
        var tagDto = new TagDTO { Id = invalidTagId, Name = "Updated Tag" };

        // Act & Assert
        Assert.ThrowsAsync<NullReferenceException>(async () => await _tagService.Update(invalidTagId, tagDto));
    }
}
