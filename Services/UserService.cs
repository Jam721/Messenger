using Messenger.Dtos;
using Messenger.Interfaces;
using Messenger.Mappers;
using Messenger.Models;
using Microsoft.AspNetCore.Identity;

namespace Messenger.Services;

public class UserService : IUserService
{
    private readonly IPasswordHash _passwordHash;
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;

    public UserService(
            IPasswordHash passwordHash, 
            IUserRepository userRepository, 
            IJwtProvider jwtProvider
        )
    {
        _passwordHash = passwordHash;
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
    }
    
    public async Task Register(RegisterUserDto userDto)
    {
        var hashPassword = _passwordHash.Generate(userDto.Password);

        var user = new User()
        {
            Id = new Guid(),
            PasswordHash = hashPassword,
            UserName = userDto.UserName,
            Email = userDto.Email
        };

        await _userRepository.AddAsync(user);
    }

    public async Task<string?> Login(string email, string password)
    {
        var user = await _userRepository.GetByEmail(email);

        if (user == null) return null;
        
        var result = _passwordHash.Verify(password, user.PasswordHash);

        if (result == false) throw new Exception("Failed to login");

        var token = _jwtProvider.GenerateToken(user);
        
        return token;
    }
}