using Messenger.Core.Models;

namespace Messenger.Core.Stores;

public interface IPostRepository
{
    Task<List<Post>?> GetAllAsync();
    Task<Post?> GetByIdAsync(int id);
    Task<Post> CreateAsync(Post post);
    Task<Post?> UpdateAsync(int id, Post post);
    Task<Post?> DeleteAsync(int id);
}