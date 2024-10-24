using System.IdentityModel.Tokens.Jwt;
using Messenger.Application.Interfaces.Services;
using Messenger.Contracts;
using Messenger.Core.Stores;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;

    public UserController(IUserService userService, IUserRepository userRepository)
    {
        _userService = userService;
        _userRepository = userRepository;
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register(UserRegisterDto registerDto)
    {
        await _userService.Register(registerDto.UserName, registerDto.Email, registerDto.Password);
        
        return Ok();
    }
    
    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login(UserLoginDto loginDto)
    {
        var token = await _userService.Login(loginDto.Email, loginDto.Password);

        if (token == null) return BadRequest("Неправильный логин/пароль");
        
        Response.Cookies.Append("tasty-cookies", token);
        
        return Ok(new {token});
    }

    [HttpGet("GetUsername")]
    public async Task<IActionResult> GetUser()
    {
        var token = Request.Cookies["tasty-cookies"];
        if (string.IsNullOrWhiteSpace(token))
            return Unauthorized();
    
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId"); // Убедитесь, что здесь правильный тип
        if (userIdClaim == null) return Unauthorized();
    
        var userId = Guid.Parse(userIdClaim.Value);
        var user = await _userRepository.GetByIdAsync(userId);

        return Ok(user!.UserName);
    }

    [HttpGet]
    [Route("GetAllPosts")]
    public async Task<IActionResult> GetAllPosts()
    {
        var token = Request.Cookies["tasty-cookies"];
        if (string.IsNullOrWhiteSpace(token))
            return Unauthorized();
    
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId"); // Убедитесь, что здесь правильный тип
        if (userIdClaim == null) return Unauthorized();
    
        var userId = Guid.Parse(userIdClaim.Value);
        var user = await _userRepository.GetByIdAsync(userId);
    
        if (user == null) return NotFound();
        
        var posts = await _userRepository.GetAllPostsAsync(userId);

        var postsDto = posts.Select(p => new
        {
            Id = p.Id,
            Title = p.Title,
            CreatedAt = p.CreatedAt,
            Description = p.Description,
            UserName = p.User.UserName
        });
        
        return Ok(postsDto);
    }
}