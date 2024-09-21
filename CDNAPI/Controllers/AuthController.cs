using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Log = Helpers.Log;

[ApiController]
[Route("v1/auth")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;

    public AuthController(ILogger<AuthController> logger, IConfiguration configuration, IUserService userService)
    {
        _logger = logger;
        _configuration = configuration;
        _userService = userService;
    }

    [HttpGet("token/{username}")]
    public async Task<IActionResult> GetToken(string username)
    {
        var user = await _userService.GetUserAsync(username);
        if (user == null) return Unauthorized();

        Log.Information(_logger, GetType().Name, $"Generating token for {username}");
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "");
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([new Claim(ClaimTypes.Name, username)]),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new { Token = tokenString });
    }
}
