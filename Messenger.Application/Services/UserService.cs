using Messenger.Application.Interfaces.Auth;
using Messenger.Application.Interfaces.Services;
using Messenger.Core.Models;
using Messenger.Core.Stores;

namespace Messenger.Application.Services;

public class UserService : IUserService
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;

    public UserService(IPasswordHasher passwordHasher, IUserRepository userRepository, IJwtProvider jwtProvider)
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
    }

    public async Task Register(string username, string email, string password)
    {
        var hashedPassword = _passwordHasher.Generate(password);

        var user = new User()
        {
            Id = Guid.NewGuid(),
            UserName = username,
            PasswordHash = hashedPassword,
            Email = email
        };

        await _userRepository.CreateAsync(user);
    }
    
    public async Task<string?> Login(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        if (user == null) return null;

        var isVerify = _passwordHasher.Verify(password, user.PasswordHash);

        if (!isVerify) throw new Exception();
        
        var token = _jwtProvider.GenerateToken(user);
        
        return token;
    }
}








