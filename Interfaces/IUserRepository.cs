using Messenger.Dtos;
using Messenger.Models;

namespace Messenger.Interfaces;

public interface IUserRepository
{
    Task<User> AddAsync(User user);
    Task<User?> GetByEmail(string email);
}