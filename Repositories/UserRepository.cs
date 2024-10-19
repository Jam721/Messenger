using Messenger.Data;
using Messenger.Dtos;
using Messenger.Interfaces;
using Messenger.Mappers;
using Messenger.Models;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<User> AddAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        
        return user;
    }

    public async Task<User?> GetByEmail(string email)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null) return null;

        return user;
    }
    
}