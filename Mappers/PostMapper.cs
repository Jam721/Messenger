using Messenger.Dtos;
using Messenger.Models;

namespace Messenger.Mappers;

public static class PostMapper
{
    public static PostDto PostToPostDto(this Post post)
    {
        return new PostDto()
        {
            Title = post.Title,
            Description = post.Description
        };
    }

    public static Post PostDtoToPost(this PostDto post)
    {
        return new Post()
        {
            Title = post.Title,
            Description = post.Description
        };
    }
}