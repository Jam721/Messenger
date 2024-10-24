using Messenger.Core.Models;

namespace Messenger.Core.Stores;

public interface IUserRepository
{
    Task<User> CreateAsync(User user);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    
    Task<List<Post?>> GetAllPostsAsync(Guid id);
}