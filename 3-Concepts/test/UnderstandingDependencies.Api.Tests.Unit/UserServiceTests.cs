using FluentAssertions;
using NSubstitute;
using UnderstandingDependencies.Api.Models;
using UnderstandingDependencies.Api.Repositories;
using UnderstandingDependencies.Api.Services;
using Xunit;

namespace UnderstandingDependencies.Api.Tests.Unit;

public class UserServiceTests
{
    // UserService has a dependencie with the UserRepository
    private readonly UserService _suv;

    // This is a mock of IUserRepository
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();

    public UserServiceTests()
    {
        _suv = new UserService(_userRepository);
    }

    [Fact]
    public async void GetAllAsync_ShouldReturnEmptyList_WhenNoUsersExist()
    {
        // Arrange
        _userRepository.GetAllAsync().Returns(Array.Empty<User>());
        // Act
        var users = await _suv.GetAllAsync();
        // Assert
        users.Should().BeEmpty();
    }

    [Fact]
    public async void GetAllAsync_ShouldReturnAListOfUsers_WhenUsersExists()
    {
        // Arrange
        var expectedUsers = new []
        {
            new User
            {
                Id = Guid.NewGuid(),
                FullName = "Daniel Aranda"
            }
        };
        _userRepository.GetAllAsync().Returns(expectedUsers);
        // Act
        var users = await _suv.GetAllAsync();
        // Assert
        users.Should().ContainSingle(x => x.FullName == "Daniel Aranda");
    }
}
