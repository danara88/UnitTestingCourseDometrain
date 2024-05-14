using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Users.Api.Contracts;
using Users.Api.Controllers;
using Users.Api.Mappers;
using Users.Api.Models;
using Users.Api.Services;
using Xunit;

namespace Users.Api.Tests.Unit;

public class UserControllerTests
{
    private readonly UserController _sut;
    private readonly IUserService _userService = Substitute.For<IUserService>();

    public UserControllerTests()
    {
        _sut = new UserController(_userService);
    }

    #region GetAll
    [Fact]
    public async Task GetAll_ShouldReturnEmptyList_WhenNoUsersExist()
    {
        // Arrange
        _userService.GetAllAsync().Returns(Enumerable.Empty<User>());

        // Act
        var result = (OkObjectResult)await _sut.GetAll();

        // Assert
        result.StatusCode.Should().Be(200);
        result.Value.As<IEnumerable<UserResponse>>().Should().BeEmpty();
    }

    [Fact]
    public async Task GetAll_ShouldReturnUsersResponse_WhenUsersExist()
    {
         // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = "Daniel Aranda"
        };
        var users = new[] { user };
        var usersResponse = users.Select(x => x.ToUserResponse());
        _userService.GetAllAsync().Returns(users);

        // Act
        var result = (OkObjectResult)await _sut.GetAll();

        // Assert
        result.StatusCode.Should().Be(200);
        result.Value.As<IEnumerable<UserResponse>>().Should().BeEquivalentTo(usersResponse);
    }
    #endregion GetAll

    #region GetById
    [Fact]
    public async Task GetById_ShouldRetunOkAndObject_WhenUserExistis()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = "Daniel Aranda"
        };
        _userService.GetByIdAsync(user.Id).Returns(user);
        var userReponse = user.ToUserResponse();

        // Act
        var result = (OkObjectResult) await _sut.GetById(user.Id);

        // Assert
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(userReponse);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenUserDoesntExist()
    {
        // Arange
        _userService.GetByIdAsync(Arg.Any<Guid>()).ReturnsNull();

        // Act
        var result = (NotFoundResult) await _sut.GetById(Guid.NewGuid());

        // Assert
        result.StatusCode.Should().Be(404);
    }
    #endregion GetById

    #region Create
    [Fact]
    public async Task Create_ShouldCreateUser_WhenUserRequestIsValid()
    {
        // Arrange
        var createUserRequest = new CreateUserRequest
        {
            FullName = "Daniel Aranda"
        };
        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = createUserRequest.FullName
        };

        // Whatever is instanciated whitin this call, we will set our user above.
        // The user inside the method will be overriding by using the user that we specified.
        _userService.CreateAsync(Arg.Do<User>(x => user = x)).Returns(true);

        // Act
        var result = (CreatedAtActionResult) await _sut.Create(createUserRequest);

        // Assert

        // This line of code must be here because the user has been set above. Arg.Do<User>(x => user = x)
        var expectedUserResponse = user.ToUserResponse();

        result.StatusCode.Should().Be(201);
        result.Value.As<UserResponse>().Should().BeEquivalentTo(expectedUserResponse);
        result.RouteValues["id"].Should().BeEquivalentTo(expectedUserResponse.Id);

        // In case you want to delete a property from the assertion
        // result.Value.As<UserResponse>().Should()
        //     .BeEquivalentTo(expectedUserResponse, options => options.Excluding(x => x.Id));
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenCreateUserRequestIsInvalid()
    {
        // Arrange
        _userService.CreateAsync(Arg.Any<User>()).Returns(false);

        // Act
        var result = (BadRequestResult) await _sut.Create(new CreateUserRequest());

        // Assert
        result.StatusCode.Should().Be(400);
    }
    #endregion Create

    #region DeleteById
    [Fact]
    public async Task DeleteById_ShouldReturnOk_WhenUserWasDeleted()
    {
        // Arrange
        _userService.DeleteByIdAsync(Arg.Any<Guid>()).Returns(true);
        // Act
        var result = (OkResult) await _sut.DeleteById(Guid.NewGuid());
        // Assert
        result.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task DeleteById_ShouldReturnNotFound_WhenUserWasntDeleted()
    {
        // Arrange
        _userService.DeleteByIdAsync(Arg.Any<Guid>()).Returns(false);
        // Act
        var result = (NotFoundResult) await _sut.DeleteById(Guid.NewGuid());
        // Assert
        result.StatusCode.Should().Be(404);
    }
    #endregion DeleteById
}
