using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Log = Helpers.Log;

namespace CDNAPI.Controllers;

[Authorize]
[ApiController]
[Route("v1/user")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;

    public UserController(ILogger<UserController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUser(string userId)
    {
        try
        {
            Log.Information(_logger, GetType().Name, $"Getting user {userId}");
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null) return NotFound(new { Message = "User not found." });

            return Ok(user);
        }
        catch (Exception ex)
        {
            Log.Error(_logger, GetType().Name, ex.Message);
            return BadRequest(new { Message = "Failed to get user." });
        }
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchUser([FromQuery] SearchUserRequest query)
    {
        try
        {
            var result = await _userService.SearchUserAsync(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            Log.Error(_logger, GetType().Name, ex.Message);
            return BadRequest(new { Message = "Unable to search" });
        }
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest data)
    {
        try
        {
            Log.Information(_logger, GetType().Name, $"Registering user {data.Username}");
            var result = await _userService.CreateUserAsync(data, User?.Identity?.Name ?? "Anonymous");
            return Ok(result);
        }
        catch (Exception ex)
        {
            Log.Error(_logger, GetType().Name, ex.Message);
            return BadRequest(new { Message = "User registration failed." });
        }
    }

    [HttpPut("{username}")]
    public async Task<IActionResult> UpdateUser(string username, [FromBody] UpdateUserRequest data)
    {
        try
        {
            Log.Information(_logger, GetType().Name, $"Updating user {username}");
            var result = await _userService.UpdateUserAsync(username, data, User?.Identity?.Name ?? "Anonymous");
            return Ok(new { Message = "User updated successfully." });
        }
        catch (Exception ex)
        {
            Log.Error(_logger, GetType().Name, ex.Message);
            return BadRequest(new { Message = "User update failed." });
        }
    }

    [HttpDelete("{username}")]
    public async Task<IActionResult> DeleteUser(string username)
    {
        try
        {
            Log.Information(_logger, GetType().Name, $"Deleting user {username}");
            var result = await _userService.DeleteUserAsync(username, User?.Identity?.Name ?? "Anonymous");
            return Ok(new { Message = "User deleted successfully." });
        }
        catch (Exception ex)
        {
            Log.Error(_logger, GetType().Name, ex.Message);
            return BadRequest(new { Message = "User deletion failed." });
        }
    }
}


