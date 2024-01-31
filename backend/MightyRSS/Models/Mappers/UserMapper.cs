using Data.Records;

namespace MightyRSS.Models.Mappers;

public static class UserMapper
{
    public static UserModel Map(UserRecord user)
    {
        return new UserModel
        {
            Reference = user.Reference,
            CreatedAt = user.CreatedAt,
            Username = user.Username
        };
    }
}