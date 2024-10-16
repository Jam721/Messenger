using Messenger.Data;
using Messenger.Dtos;
using Messenger.Enums;
using Messenger.Helpers.Post;
using Messenger.Interfaces;
using Messenger.Models;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Repositories;

public class PostRepository : IPostRepositortes
{
    private readonly AppDbContext _dbContext;

    public PostRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> GetLastIdAsync()
    {
        var posts = await _dbContext.Posts.ToListAsync();
        
        if (posts.Count == 0) return 1;
        
        return posts.Count + 1;
    }

    public async Task<List<Post>> GetAllAsync(PostQuery query)
    {
        var posts = _dbContext.Posts.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Title))
            posts = posts.Where(p => p.Title == query.Title);

        
        if (query.PostEnum == PostEnum.Id)
            posts = query.Descending ? 
                posts.OrderByDescending(p => p.Id) : posts.OrderBy(p => p.Id);
        
        if (query.PostEnum == PostEnum.Description)
            posts = query.Descending ? 
                posts.OrderByDescending(p => p.Description) : posts.OrderBy(p => p.Description);
        
        if (query.PostEnum == PostEnum.Title)
            posts = query.Descending ? 
                posts.OrderByDescending(p => p.Title) : posts.OrderBy(p => p.Title);
        
        if (query.PostEnum == PostEnum.CreatedAt)
            posts = query.Descending ? 
                posts.OrderByDescending(p => p.CreatedAt) : posts.OrderBy(p => p.CreatedAt);
        
        
        var skipNumber = (query.PageNumber - 1) * query.PageSize;

        return await posts.Skip(skipNumber).Take(query.PageSize).ToListAsync();
    }

    public async Task<Post> CreateAsync(Post post)
    {
        await _dbContext.Posts.AddAsync(post);
        await _dbContext.SaveChangesAsync();

        return post;
    }

    public async Task<Post?> UpdateAsync(int id, PostDto postDto)
    {
        var post = await _dbContext.Posts.FirstOrDefaultAsync(p=>p.Id==id);

        if (post == null) return null;

        post.Description = postDto.Description;
        post.Title = postDto.Title;

        _dbContext.Posts.Update(post);
        await _dbContext.SaveChangesAsync();

        return post;
    }

    public async Task<Post?> DeleteAsync(int id)
    {
        var post = await _dbContext.Posts.FirstOrDefaultAsync(p=>p.Id==id);

        if (post == null) return null;

        _dbContext.Posts.Remove(post);
        await _dbContext.SaveChangesAsync();

        return post;
    }
}