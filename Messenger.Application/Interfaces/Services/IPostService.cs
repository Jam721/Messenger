namespace Messenger.Application.Interfaces;

public interface IPostService
{
    Task<int> GetNextPostId();
}