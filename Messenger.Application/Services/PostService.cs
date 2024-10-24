using Messenger.Application.Interfaces;
using Messenger.Persistence.DataBases;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Application.Services;

public class PostService : IPostService
{
    private readonly AppDbContext _dbContext;

    public PostService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> GetNextPostId()
    {
        var lastPost = await _dbContext.Posts
            .AsNoTracking()
            .OrderByDescending(p => p.Id)
            .FirstOrDefaultAsync();

        if (lastPost == null) return 1;
        
        return lastPost.Id+1;
    }
}