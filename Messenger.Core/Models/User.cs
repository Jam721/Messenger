namespace Messenger.Core.Models;

public class User
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public List<Post> Posts { get; set; } = new();
}