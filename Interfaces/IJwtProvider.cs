using Messenger.Models;

namespace Messenger.Interfaces;

public interface IJwtProvider
{
    string GenerateToken(User user);
}