using Messenger.Dtos;
using Messenger.Helpers.Post;
using Messenger.Models;

namespace Messenger.Interfaces;

public interface IPostRepositortes
{
    Task<int> GetLastIdAsync();
    Task<List<Post>> GetAllAsync(PostQuery query);
    Task<Post> CreateAsync(Post post);
    Task<Post?> UpdateAsync(int id, PostDto postDto);
    Task<Post?> DeleteAsync(int id);
}