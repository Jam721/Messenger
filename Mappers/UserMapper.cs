using Messenger.Dtos;
using Messenger.Models;

namespace Messenger.Mappers;

public static class UserMapper
{
    public static User UserDtoToUser(this UserDto userDto)
    {
        return new User()
        {
            UserName = userDto.UserName,
            Email = userDto.PasswordHash
        };
    }

    public static UserDto UserDtoToUser(this User user)
    {
        return new UserDto()
        {
            UserName = user.UserName,
            Email = user.Email
        };
    }
}