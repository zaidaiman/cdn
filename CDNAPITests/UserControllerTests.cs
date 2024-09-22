using System.ComponentModel.DataAnnotations;
using CDNAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

public class UserControllerTests
{
    private readonly UserController _controller;
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<ILogger<UserController>> _mockLogger;

    public UserControllerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _mockLogger = new Mock<ILogger<UserController>>();
        _controller = new UserController(_mockLogger.Object, _mockUserService.Object);
    }

    [Fact]
    public void RegisterUser_InvalidBody_ReturnsBadRequest()
    {
        var data = new RegisterUserRequest { Username = "testuser" };
        var context = new ValidationContext(data);
        var results = new List<ValidationResult>();
        var valid = Validator.TryValidateObject(data, context, results, true);

        Assert.False(valid);
        Assert.True(results.Count > 0);
    }

    [Fact]
    public void RegisterUser_InvalidListString_ReturnsBadRequest()
    {
        var data = new RegisterUserRequest { Username = "testuser", Email = "me@example.com", PhoneNumber = "1234567890", Skillsets = new List<string> { "" }, Hobby = new List<string> { "" } };
        var context = new ValidationContext(data);
        var results = new List<ValidationResult>();
        var valid = Validator.TryValidateObject(data, context, results, true);

        Assert.False(valid);
        Assert.True(results.Count > 0);
    }

    [Fact]
    public async Task RegisterUser_ReturnsOkResult()
    {
        var data = new RegisterUserRequest { Username = "testuser" };
        var expectedResult = new UserResponse { };
        _mockUserService.Setup(service => service.CreateUserAsync(data, It.IsAny<string>())).ReturnsAsync(expectedResult);

        var result = await _controller.RegisterUser(data);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResult, okResult.Value);
    }

    [Fact]
    public async Task GetUser_ReturnsOkResult()
    {
        var userId = "testuser";
        var expectedResult = new UserResponse { };
        _mockUserService.Setup(service => service.GetUserByIdAsync(userId)).ReturnsAsync(expectedResult);

        var result = await _controller.GetUser(userId);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResult, okResult.Value);
    }

    [Fact]
    public void UpdateUser_InvalidBody_ReturnsBadRequest()
    {
        var data = new UpdateUserRequest { Email = "me" };
        var context = new ValidationContext(data);
        var results = new List<ValidationResult>();
        var valid = Validator.TryValidateObject(data, context, results, true);

        Assert.False(valid);
        Assert.True(results.Count > 0);
    }

    [Fact]
    public async Task UpdateUser_ReturnsOkResult()
    {
        var username = "testuser";
        var email = "me@example.com";
        var data = new UpdateUserRequest { Email = email };
        var expectedResult = new UserResponse { };
        _mockUserService.Setup(service => service.UpdateUserAsync(username, data, It.IsAny<string>())).ReturnsAsync(expectedResult);

        var result = await _controller.UpdateUser(username, data);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResult, okResult.Value);
    }

    // [Fact]
    // public async Task SearchUser_ReturnsOkResult()
    // {
    //     var query = "testuser";
    //     var page = 1;
    //     var size = 10;
    //     var sorts = new List<string> { "username" };
    //     var expectedResult = new SearchUserResponse { };
    //     _mockUserService.Setup(service => service.SearchUserAsync(new SearchUserRequest
    //     {
    //         Search = query,
    //         Page = page,
    //         Size = size,
    //         Sort = sorts
    //     })).ReturnsAsync(expectedResult);

    //     var result = await _controller.SearchUser(query, page, size, sorts);
    //     var okResult = Assert.IsType<OkObjectResult>(result);
    //     Assert.Equal(expectedResult, okResult.Value);
    // }
}
