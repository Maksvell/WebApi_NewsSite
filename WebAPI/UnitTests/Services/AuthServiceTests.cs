using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BLL.DTOs;
using BLL.Services;
using DAL.Entities;
using DAL.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;

namespace UnitTests.Services;

[TestFixture]
public class AuthServiceTests
{
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private AuthService _authService;

    [SetUp]
    public void Setup()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _authService = new AuthService(_mockUnitOfWork.Object);
    }

    [Test]
    public async Task GetEntity_WithValidLoginRequest_ReturnsAuthor()
    {
        // Arrange
        var validLoginRequest = new LoginRequest { Email = "test@example.com", Password = "password" };
        var mockAuthor = new Author { Id = 1, Email = "test@example.com", Password = "hashedPassword" };

        _mockUnitOfWork.Setup(u => u.AuthorRepository.GetByEmailAndPassword(validLoginRequest.Email, validLoginRequest.Password))
                       .ReturnsAsync(mockAuthor);

        // Act
        var result = await _authService.GetEntity(validLoginRequest);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Email.Should().Be("test@example.com");
    }

    [Test]
    public void GetEntity_WithInvalidLoginRequest_ThrowsException()
    {
        // Arrange
        var invalidLoginRequest = new LoginRequest { Email = "invalid@example.com", Password = "invalidPassword" };
        _mockUnitOfWork.Setup(u => u.AuthorRepository.GetByEmailAndPassword(invalidLoginRequest.Email, invalidLoginRequest.Password))
                       .ReturnsAsync((Author)null);

        // Act & Assert
        Assert.ThrowsAsync<NullReferenceException>(async () => await _authService.GetEntity(invalidLoginRequest));
    }
    [Test]
    public void CreateToken_WithValidAuthor_ReturnsToken()
    {
        // Arrange
        var validAuthor = new Author { Id = 1, Email = "test@example.com" };

        // Act
        var result = _authService.CreateToken(validAuthor);

        // Assert
        result.Should().NotBeNullOrEmpty();
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadToken(result) as JwtSecurityToken;
        token.Should().NotBeNull();

        token?.Claims.Should().NotBeNull().And.HaveCountGreaterOrEqualTo(2);;
    }
}