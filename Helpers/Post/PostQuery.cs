using Messenger.Enums;

namespace Messenger.Helpers.Post;

public class PostQuery
{
    public string Title { get; set; } = string.Empty;

    public PostEnum PostEnum { get; set; } = PostEnum.Id;
    public bool Descending { get; set; } = false;

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}