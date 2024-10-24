using Messenger.Core.Models;

namespace Messenger.Application.Interfaces.Auth;

public interface IJwtProvider
{
    string GenerateToken(User user);
}