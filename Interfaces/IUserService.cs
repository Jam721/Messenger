using Messenger.Dtos;

namespace Messenger.Interfaces;

public interface IUserService
{
    Task Register(RegisterUserDto userDto);
    Task<string?> Login(string email, string password);
}