using DiscountSystem.Domain.Entities;
using DiscountSystem.Domain.Enums;
using DiscountSystem.Infrastructure;
using DiscountSystem.Infrastructure.Identity;
using DiscountSystem.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DiscountSystem.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly TokenProvider _tokenProvider;

    public AuthController(UserManager<User> userManager, TokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var user = new User
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            UserName = model.Email
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        await _userManager.AddToRoleAsync(user, ApplicationRole.User.ToIdentityRole());

        return Ok(new { Message = "User registered successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
        {
            return Unauthorized();
        }

        var isValidPassword = await _userManager.CheckPasswordAsync(user, model.Password);

        if (!isValidPassword) 
        {
            return Unauthorized();
        }

        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenProvider.GenerateJwtToken(user, roles);
        var refreshToken = await _tokenProvider.GenerateRefreshToken(user.Id);

        return Ok(new { Token = token, RefreshToken = refreshToken });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel model)
    {
        try
        {
            var (newJwtToken, newRefreshToken) = await _tokenProvider.RefreshTokens(model.RefreshToken);

            return Ok(new { Token = newJwtToken, RefreshToken = newRefreshToken });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { Error = ex.Message });
        }
    }
}
