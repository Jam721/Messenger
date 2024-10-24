using Messenger.Core.Models;
using Messenger.Core.Stores;
using Messenger.Persistence.DataBases;
using Messenger.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Persistence.Repositories;

public class PostRepository : IPostRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IUserRepository _userRepository;

    public PostRepository(AppDbContext dbContext, IUserRepository userRepository)
    {
        _dbContext = dbContext;
        _userRepository = userRepository;
    }

    public async Task<List<Post>?> GetAllAsync()
    {
        var postEntities = await _dbContext.Posts
            .Include(p => p.User) 
            .AsNoTracking()
            .ToListAsync();
        
        var posts = postEntities.Select(p => new Post()
        {
            Id = p.Id,
            Title = p.Title,
            Description = p.Description,
            CreatedAt = p.CreatedAt,
            User = new User
            {
                Id = p.User.Id,
                UserName = p.User.UserName,
                Email = p.User.Email,
                PasswordHash = p.User.PasswordHash,
            }
        }).ToList();

        return posts;
    }

    public async Task<Post?> GetByIdAsync(int id)
    {
        var postEntity = await _dbContext.Posts
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (postEntity == null) return null;
        
        return new Post
        {
            Id = postEntity.Id,
            Description = postEntity.Description,
            Title = postEntity.Title,
            CreatedAt = postEntity.CreatedAt
        };
    }

    public async Task<Post> CreateAsync(Post post)
    {
        if (post == null) throw new ArgumentNullException(nameof(post));
        if (post.User == null || post.User.Id == Guid.Empty)
            throw new Exception("User information is required.");
        
        var user = await _dbContext.Users.FindAsync(post.User.Id);

        if (user == null)
        {
            throw new Exception("User not found."); 
        }
        
        var newPost = new PostEntity()
        {
            Title = post.Title,
            Description = post.Description,
            CreatedAt = DateTime.UtcNow,
            User = user 
        };


        await _dbContext.Posts.AddAsync(newPost);
        await _dbContext.SaveChangesAsync();
        
        return new Post()
        {
            Id = newPost.Id,
            Title = newPost.Title,
            Description = newPost.Description,
            CreatedAt = newPost.CreatedAt,
            User = new User
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                Posts = user.Posts.Select(p => new Post
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    CreatedAt = p.CreatedAt
                }).ToList() 
            }
        };
    }

    public async Task<Post?> UpdateAsync(int id, Post post)
    {
        var newPostEntity = await _dbContext.Posts
            .FirstOrDefaultAsync(p => p.Id == id);

        if (newPostEntity == null) return null;
        
        newPostEntity.Description = post.Description;
        newPostEntity.Title = post.Title;
        
        await _dbContext.SaveChangesAsync();

        return new Post
        {
            Id = newPostEntity.Id,
            Description = newPostEntity.Description,
            Title = newPostEntity.Title,
            CreatedAt = newPostEntity.CreatedAt,
            User = new User
            {
                Id = newPostEntity.User.Id,
                UserName = newPostEntity.User.UserName,
                Email = newPostEntity.User.Email,
                PasswordHash = newPostEntity.User.PasswordHash,
            }
        };
    }

    public async Task<Post?> DeleteAsync(int id)
    {
        var postEntity = await _dbContext.Posts
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (postEntity == null) return null;

        _dbContext.Posts.Remove(postEntity);
        await _dbContext.SaveChangesAsync();

        return new Post
        {
            Id = postEntity.Id,
            Description = postEntity.Description,
            Title = postEntity.Title,
            CreatedAt = postEntity.CreatedAt,
            User = new User
            {
                Id = postEntity.User.Id,
                UserName = postEntity.User.UserName,
                Email = postEntity.User.Email,
                PasswordHash = postEntity.User.PasswordHash,
            }
        };
    }
}