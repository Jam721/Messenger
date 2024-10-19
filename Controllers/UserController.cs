using Messenger.Dtos;
using Messenger.Interfaces;
using Messenger.Services;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;

    public UserController(IUserRepository userRepository, IUserService userService)
    {
        _userRepository = userRepository;
        _userService = userService;
    }
    
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody]RegisterUserDto userDto)
    {
        await _userService.Register(userDto);

        return Ok();
    }

    [HttpGet("{email}")]
    public async Task<IActionResult> GetByEmail([FromRoute]string email)
    {
        var user = await _userRepository.GetByEmail(email);

        if (user == null) return BadRequest("No email");

        return Ok(user);
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginUserDto loginUserDto)
    {
        var token = await _userService.Login(loginUserDto.Email, loginUserDto.Password);

        if (token == null) return BadRequest("Не получилось залогиниться");
        
        Response.Cookies.Append("tasty-cookies", token);
        
        return Ok(token);
    }
}