namespace Messenger.Core.Models;

public class Post
{
    public int Id { get; set; }
    
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = new();
}