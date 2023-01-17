namespace Core.Models.Mappers;

public static class UserModelMapper
{
    public static UserModel Map(UserModel user)
    {
        return new UserModel
        {
            Reference = user.Reference,
            Username = user.Username
        };
    }
}