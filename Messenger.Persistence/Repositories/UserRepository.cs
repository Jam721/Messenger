using Messenger.Core.Models;
using Messenger.Core.Stores;
using Messenger.Persistence.DataBases;
using Messenger.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> CreateAsync(User user)
    {
        var userEntity = new UserEntity()
        {
            Id = user.Id,
            UserName = user.UserName,
            PasswordHash = user.PasswordHash,
            Email = user.Email
        };
        await _dbContext.Users.AddAsync(userEntity);
        await _dbContext.SaveChangesAsync();
        
        return user;
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u=>u.Id==id);
        

        return new User()
        {
            Id = user!.Id,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            UserName = user.UserName,
            Posts = user.Posts.Select(post => new Post
            {
                Id = post.Id,
                Title = post.Title,
                Description = post.Description,
                CreatedAt = post.CreatedAt,
            }).ToList()
            
        };
    }

    public async Task<List<Post?>> GetAllPostsAsync(Guid id)
    {
        var postsEntities = await _dbContext.Posts
            .AsNoTracking()
            .Where(p => p.User.Id == id)
            .ToListAsync();
        
        var posts = postsEntities.Select(p => new Post()
        {
            Id = p.Id,
            CreatedAt = p.CreatedAt,
            Description = p.Description,
            Title = p.Title,
            User = GetByIdAsync(id).Result
        }).ToList();
        
        return posts!;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var userEntity = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);

        if (userEntity == null) return null;
        
        return new User()
        {
            Id = userEntity.Id,
            UserName = userEntity.UserName,
            Email = userEntity.Email,
            PasswordHash = userEntity.PasswordHash
        };
    }
}