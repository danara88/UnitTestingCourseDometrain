using FluentAssertions;
using Microsoft.Data.Sqlite;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using Users.Api.Logging;
using Users.Api.Models;
using Users.Api.Repositories;
using Users.Api.Services;
using Xunit;

namespace Users.Api.Tests.Unit;

public class UserServiceTests
{
    private readonly UserService _sut;
    private readonly IUserRepository _userRepository= Substitute.For<IUserRepository>();
    private readonly ILoggerAdapter<UserService> _logger = Substitute.For<ILoggerAdapter<UserService>>();

    public UserServiceTests()
    {
        _sut = new UserService(_userRepository, _logger);
    }

    #region GetAllAsync
    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoUsersExist()
    {
        // Arrange
        _userRepository.GetAllAsync().Returns(Enumerable.Empty<User>());
        // Act
        var result = await _sut.GetAllAsync();
        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnUsers_WhenSomeUsersExist()
    {
        // Arrange
        var danielaranda = new User
        {
            Id = Guid.NewGuid(),
            FullName = "Daniel Aranda"
        };
        var expectedUsers = new[]
        {
           danielaranda
        };
        _userRepository.GetAllAsync().Returns(expectedUsers);

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Single().Should().BeEquivalentTo(danielaranda);
        result.Should().BeEquivalentTo(expectedUsers);
    }

    [Fact]
    public async Task GetAllAsync_ShouldLogMessages_WhenInvoked()
    {
        // Arrange
        _userRepository.GetAllAsync().Returns(Enumerable.Empty<User>());
        // Act
        await _sut.GetAllAsync();
        // Assert
        // This mean tha the logger was called one time with expected param
        //_logger.Received(1).LogInformation(Arg.Is<string>(x => x!.StartsWith("Retrieving")));
        _logger.Received(1).LogInformation(Arg.Is("Retrieving all users"));
        _logger.Received(1).LogInformation(Arg.Is("All users retrieved in {0}ms"), Arg.Any<long>());
    }

    [Fact]
    public async Task GetAllAsync_ShouldLogMessageAndException_WhenExceptionIsThrown()
    {
        // Arrange
        var sqlException = new SqliteException("Something went wrong", 500);
        _userRepository.GetAllAsync().Throws(sqlException);
        // Act
       var requestAction = async () => await _sut.GetAllAsync();
        // Assert
       await requestAction.Should()
          .ThrowAsync<SqliteException>()
          .WithMessage("Something went wrong");
        _logger.Received(1).LogError(Arg.Is(sqlException), Arg.Is("Something went wrong while retrieving all users"));

    }
    #endregion GetAllAsync

    #region GetByIdAsync
    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        // Assert
        var existingUser = new User
        {
            Id = Guid.NewGuid(),
            FullName = "Daniel Aranda"
        };
        _userRepository.GetByIdAsync(existingUser.Id).Returns(existingUser);

        // Act
        var result = await _sut.GetByIdAsync(existingUser.Id);

        // Assert
        result.Should().BeEquivalentTo(existingUser);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNoUserExists()
    {
        // Assert
        _userRepository.GetByIdAsync(Arg.Any<Guid>()).ReturnsNull();

        // Act
        var result = await _sut.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldLogMessages_WhenInvoked()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userRepository.GetByIdAsync(userId).ReturnsNull();
        // Act
        await _sut.GetByIdAsync(userId);
        // Assert
        _logger.Received(1)
            .LogInformation(
                Arg.Is("Retrieving user with id: {0}"),
                Arg.Is(userId));
        _logger.Received(1)
            .LogInformation(
              Arg.Is("User with id {0} retrieved in {1}ms"),
              Arg.Is(userId),
              Arg.Any<long>());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldLogMessagesAndException_WhenExceptionIsThrown()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var sqlException = new SqliteException("Something went wrong", 500);
        _userRepository.GetByIdAsync(userId).Throws(sqlException);

        // Act
        var requestAction = async () => await _sut.GetByIdAsync(userId);

        // Assert
        await requestAction.Should()
            .ThrowAsync<SqliteException>()
            .WithMessage("Something went wrong");
        _logger.Received(1)
            .LogError(
              Arg.Is(sqlException),
              Arg.Is("Something went wrong while retrieving user with id {0}"), userId);
    }
    #endregion GetByIdAsync

    #region CreateAsync
    [Fact]
    public async Task CreateAsync_ShouldCreateUser_WhenDetailsAreValid()
    {
        // Arrange
        var userToCreate = new User
        {
            Id = Guid.NewGuid(),
            FullName = "Danile Aranda"
        };
        _userRepository.CreateAsync(userToCreate).Returns(true);

        // Act
        var result = await _sut.CreateAsync(userToCreate);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task CreateAsync_ShouldLogMessages_WhenInvoked()
    {
        // Arrange
        var userToCreate = new User
        {
            Id = Guid.NewGuid(),
            FullName = "Danile Aranda"
        };
        _userRepository.CreateAsync(userToCreate).Returns(true);

        // Act
        await _sut.CreateAsync(userToCreate);

        // Assert
        _logger.Received(1).LogInformation(
            Arg.Is("Creating user with id {0} and name: {1}"),
            Arg.Is(userToCreate.Id),
            Arg.Is(userToCreate.FullName));
       _logger.Received(1).LogInformation(
            Arg.Is("User with id {0} created in {1}ms"),
            Arg.Is(userToCreate.Id),
            Arg.Any<long>());
    }

    [Fact]
    public async Task CreateAsync_ShouldLogMessagesAndException_WhenExceptionIsThrown()
    {
        // Arrange
        var sqlException = new SqliteException("Something went wrong", 500);
        var userToCreate = new User
        {
            Id = Guid.NewGuid(),
            FullName = "Danile Aranda"
        };
        _userRepository.CreateAsync(userToCreate).Throws(sqlException);

        // Act
        var requestAction = async () => await _sut.CreateAsync(userToCreate);

        // Assert
        await requestAction
            .Should()
            .ThrowAsync<SqliteException>()
            .WithMessage("Something went wrong");
        _logger.Received(1)
            .LogError(
              Arg.Is(sqlException),
              Arg.Is("Something went wrong while creating a user"));
    }
    #endregion CreateAsync

    #region DeleteAsync
    [Fact]
    public async Task DeleteAsync_ShouldDeleteUser_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userRepository.DeleteByIdAsync(userId).Returns(true);

        // Act
        var result = await _sut.DeleteByIdAsync(userId);

        // Assert
        result.Should().BeTrue();
      }

    [Fact]
    public async Task DeleteAsync_ShouldNotDeleteUser_WhenUserDoesntExist()
    {
        // Arrange
         var userId = Guid.NewGuid();
        _userRepository.DeleteByIdAsync(userId).Returns(false);

        // Act
        var result = await _sut.DeleteByIdAsync(userId);

        // Assert
         result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_ShouldLogMessages_WhenInvoke()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userRepository.DeleteByIdAsync(userId).Returns(true);

        // Act
        await _sut.DeleteByIdAsync(userId);

        // Assert
        _logger.Received(1).LogInformation(
            Arg.Is("Deleting user with id: {0}"),
            Arg.Is(userId));
        _logger.Received(1).LogInformation(
            Arg.Is("User with id {0} deleted in {1}ms"),
            Arg.Is(userId),
            Arg.Any<long>());
    }

    [Fact]
    public async void DeleteAsync_ShouldLogMessagesAndException_WhenExceptionIsThrown()
    {
        // Arrange
        var sqlException = new SqliteException("Something went wrong", 500);
        var userId = Guid.NewGuid();
        _userRepository.DeleteByIdAsync(userId).Throws(sqlException);

        // Act
        var requestAction = async () => await _sut.DeleteByIdAsync(userId);

        // Assert
        await requestAction
            .Should()
            .ThrowAsync<SqliteException>()
            .WithMessage("Something went wrong");
        _logger.Received(1)
            .LogError(
              Arg.Is(sqlException),
              Arg.Is("Something went wrong while deleting user with id {0}"),
              Arg.Is(userId));
    }
    #endregion DeleteAsync
}

