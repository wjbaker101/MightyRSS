using Core.Models.Mappers;
using Data.Records;
using Data.UoW;
using MightyRSS.Api.Auth;
using MightyRSS.Api.User.Types;
using MightyRSS.Types;
using NetApiLibs.Type;
using System;

namespace MightyRSS.Api.User;

public interface IUserService
{
    Result<GetSelfResponse> GetSelf(IRequestContext requestContext);
    Result<GetUserResponse> GetUser(Guid reference);
    Result<CreateUserResponse> CreateUser(CreateUserRequest request);
}

public sealed class UserService : IUserService
{
    private readonly IUnitOfWorkFactory<IMightyUnitOfWork> _mightyUnitOfWorkFactory;
    private readonly IPasswordService _passwordService;

    public UserService(IUnitOfWorkFactory<IMightyUnitOfWork> mightyUnitOfWorkFactory, IPasswordService passwordService)
    {
        _mightyUnitOfWorkFactory = mightyUnitOfWorkFactory;
        _passwordService = passwordService;
    }

    public Result<GetSelfResponse> GetSelf(IRequestContext requestContext)
    {
        var self = requestContext.User;

        return new GetSelfResponse
        {
            User = UserMapper.Map(self)
        };
    }

    public Result<GetUserResponse> GetUser(Guid reference)
    {
        using var unitOfWork = _mightyUnitOfWorkFactory.Create();

        var userResult = unitOfWork.Users.GetByReference(reference);
        if (!userResult.TrySuccess(out var user))
            return Result<GetUserResponse>.FromFailure(userResult);

        unitOfWork.Commit();

        return new GetUserResponse
        {
            Reference = user.Reference,
            Username = user.Username
        };
    }

    public Result<CreateUserResponse> CreateUser(CreateUserRequest request)
    {
        var userReference = Guid.NewGuid();
        var passwordSalt = Guid.NewGuid();
        var hashedPassword = _passwordService.HashPassword(request.Password, passwordSalt);

        using var unitOfWork = _mightyUnitOfWorkFactory.Create();

        var user = new UserRecord
        {
            Reference = userReference,
            CreatedAt = DateTime.UtcNow,
            Username = request.Username,
            Password = hashedPassword,
            PasswordSalt = passwordSalt
        };

        unitOfWork.Users.Save(user);

        unitOfWork.Commit();

        return new CreateUserResponse
        {
            Reference = user.Reference,
            Username = user.Username
        };
    }
}