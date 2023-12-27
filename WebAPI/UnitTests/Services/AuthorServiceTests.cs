using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTOs;
using BLL.Services;
using DAL.Entities;
using DAL.Interfaces;
using Moq;
using NUnit.Framework;

namespace UnitTests.Services;

[TestFixture]
public class AuthorServiceTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IMapper> _mapperMock;
    private AuthorService _authorService;

    [SetUp]
    public void SetUp()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _authorService = new AuthorService(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Test]
    public async Task GetAll_ShouldReturnListOfAuthors()
    {
        // Arrange
        var authorEntities = new List<Author> { new Author { Id = 1, Name = "Author 1" } };
        var authorDTOs = new List<AuthorDTO> { new AuthorDTO { Id = 1, Name = "Author 1" } };

        _unitOfWorkMock.Setup(u => u.AuthorRepository.GetAll()).ReturnsAsync(authorEntities);
        _mapperMock.Setup(m => m.Map<IEnumerable<Author>, IEnumerable<AuthorDTO>>(authorEntities))
            .Returns(authorDTOs);

        // Act
        var result = await _authorService.GetAll();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(authorDTOs.Count()));
        Assert.That(result, Is.EquivalentTo(authorDTOs));
    }

    [Test]
    public async Task GetById_ExistingId_ShouldReturnAuthorDTO()
    {
        // Arrange
        var authorEntity = new Author { Id = 1, Name = "Author 1" };
        var authorDTO = new AuthorDTO { Id = 1, Name = "Author 1" };

        _unitOfWorkMock.Setup(u => u.AuthorRepository.GetById(It.IsAny<int>())).ReturnsAsync(authorEntity);
        _mapperMock.Setup(m => m.Map<AuthorDTO>(authorEntity)).Returns(authorDTO);

        // Act
        var result = await _authorService.GetById(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(authorDTO.Id));
        Assert.That(result.Name, Is.EqualTo(authorDTO.Name));
    }

    [Test]
    public void GetById_NonExistingId_ShouldThrowNullReferenceException()
    {
        // Arrange
        _unitOfWorkMock.Setup(u => u.AuthorRepository.GetById(It.IsAny<int>())).ReturnsAsync((Author)null);

        // Act & Assert
        Assert.ThrowsAsync<NullReferenceException>(() => _authorService.GetById(1));
    }

    [Test]
    public async Task Add_ValidAuthorDTO_ShouldAddAuthor()
    {
        // Arrange
        var authorDTO = new AuthorDTO { Name = "New Author", Email = "newauthor@example.com", Password = "password" };

        _mapperMock.Setup(m => m.Map<Author>(authorDTO)).Returns(new Author());
        _unitOfWorkMock.Setup(u => u.AuthorRepository.Add(It.IsAny<Author>()));
        _unitOfWorkMock.Setup(u => u.AuthorRepository.GetAll()).ReturnsAsync(new List<Author>());

        // Act
        await _authorService.Add(authorDTO);

        // Assert
        _unitOfWorkMock.Verify(u => u.AuthorRepository.Add(It.IsAny<Author>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task Add_DuplicateEmail_ShouldThrowException()
    {
        // Arrange
        var authorDTO = new AuthorDTO { Name = "Existing Author", Email = "existingauthor@example.com", Password = "password" };
        var existingAuthors = new List<Author> { new Author { Id = 1, Email = "existingauthor@example.com" } };

        _mapperMock.Setup(m => m.Map<Author>(authorDTO)).Returns(new Author());
        _unitOfWorkMock.Setup(u => u.AuthorRepository.GetAll()).ReturnsAsync(existingAuthors);

        // Act & Assert
        Assert.ThrowsAsync<Exception>(() => _authorService.Add(authorDTO));
    }

    [Test]
    public async Task Update_ExistingIdAndValidAuthorDTO_ShouldUpdateAuthor()
    {
        // Arrange
        var authorDTO = new AuthorDTO { Name = "Updated Author", Email = "updatedauthor@example.com", Password = "newpassword" };
        var existingAuthor = new Author { Id = 1, Name = "Existing Author", Email = "existingauthor@example.com", Password = "password" };

        _unitOfWorkMock.Setup(u => u.AuthorRepository.GetById(1)).ReturnsAsync(existingAuthor);
        _mapperMock.Setup(m => m.Map<Author>(authorDTO)).Returns(existingAuthor);

        // Act
        await _authorService.Update(1, authorDTO);

        // Assert
        Assert.That(existingAuthor.Name, Is.EqualTo(authorDTO.Name));
        Assert.That(existingAuthor.Email, Is.EqualTo(authorDTO.Email));
        Assert.That(existingAuthor.Password, Is.EqualTo(authorDTO.Password));
        _unitOfWorkMock.Verify(u => u.AuthorRepository.Update(existingAuthor), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public void Update_NonExistingId_ShouldThrowNullReferenceException()
    {
        // Arrange
        _unitOfWorkMock.Setup(u => u.AuthorRepository.GetById(It.IsAny<int>())).ReturnsAsync((Author)null);

        // Act & Assert
        Assert.ThrowsAsync<NullReferenceException>(() => _authorService.Update(1, new AuthorDTO()));
    }

    [Test]
    public void Delete_ExistingId_ShouldDeleteAuthor()
    {
        // Arrange
        var existingAuthor = new Author { Id = 1, Name = "Existing Author", Email = "existingauthor@example.com", Password = "password" };
        _unitOfWorkMock.Setup(u => u.AuthorRepository.GetById(1)).ReturnsAsync(existingAuthor);

        // Act
        Assert.DoesNotThrowAsync(() => _authorService.Delete(1));

        // Assert
        _unitOfWorkMock.Verify(u => u.AuthorRepository.Delete(existingAuthor), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public void Delete_NonExistingId_ShouldThrowNullReferenceException()
    {
        // Arrange
        _unitOfWorkMock.Setup(u => u.AuthorRepository.GetById(It.IsAny<int>())).ReturnsAsync((Author)null);

        // Act & Assert
        Assert.ThrowsAsync<NullReferenceException>(() => _authorService.Delete(1));
    }

    [Test]
    public async Task CheckIfRegistered_ExistingEmail_ShouldReturnTrue()
    {
        // Arrange
        var existingEmail = "existingauthor@example.com";
        var existingAuthors = new List<Author> { new Author { Id = 1, Email = existingEmail } };

        _unitOfWorkMock.Setup(u => u.AuthorRepository.GetAll()).ReturnsAsync(existingAuthors);

        // Act
        var result = await _authorService.CheckIfRegistered(existingEmail);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task CheckIfRegistered_NonExistingEmail_ShouldReturnFalse()
    {
        // Arrange
        var nonExistingEmail = "nonexistingauthor@example.com";
        var existingAuthors = new List<Author> { new Author { Id = 1, Email = "existingauthor@example.com" } };

        _unitOfWorkMock.Setup(u => u.AuthorRepository.GetAll()).ReturnsAsync(existingAuthors);

        // Act
        var result = await _authorService.CheckIfRegistered(nonExistingEmail);

        // Assert
        Assert.That(result, Is.False);
    }
}
