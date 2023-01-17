using MightyRSS._Api.Auth.Types;
using MightyRSS.Auth;
using MightyRSS.Auth.Types;
using MightyRSS.Data.Records;
using MightyRSS.Data.UoW;
using NetApiLibs.Type;
using System;
using System.Net;

namespace MightyRSS._Api.Auth;

public interface IAuthService
{
    Result<GetUserResponse> GetUser(Guid reference);
    Result<CreateUserResponse> CreateUser(CreateUserRequest request);
    Result<LogInResponse> LogIn(LogInRequest request);
}

public sealed class AuthService : IAuthService
{
    private readonly IUnitOfWorkFactory<IMightyUnitOfWork> _mightyUnitOfWork;
    private readonly IPasswordHelper _passwordHelper;
    private readonly IJwtHelper _jwtHelper;

    public AuthService(
        IUnitOfWorkFactory<IMightyUnitOfWork> mightyUnitOfWork,
        IPasswordHelper passwordHelper,
        IJwtHelper jwtHelper)
    {
        _mightyUnitOfWork = mightyUnitOfWork;
        _passwordHelper = passwordHelper;
        _jwtHelper = jwtHelper;
    }

    public Result<GetUserResponse> GetUser(Guid reference)
    {
        using var unitOfWork = _mightyUnitOfWork.Create();

        var user = unitOfWork.Users.GetByReference(reference);
        if (user == null)
            return Result<GetUserResponse>.Failure("User with the given reference could not be found.", HttpStatusCode.NotFound);

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
        var hashedPassword = _passwordHelper.HashPassword(request.Password, passwordSalt);

        using var unitOfWork = _mightyUnitOfWork.Create();

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

    public Result<LogInResponse> LogIn(LogInRequest request)
    {
        using var unitOfWork = _mightyUnitOfWork.Create();

        var user = unitOfWork.Users.GetByUsername(request.Username);
        if (user == null)
            return Result<LogInResponse>.Failure("User with the given username could not be found, please check and try again.", HttpStatusCode.Unauthorized);
        if (!_passwordHelper.IsMatch(user.Password, request.Password, user.PasswordSalt))
            return null;

        var jwtToken = _jwtHelper.CreateToken(new AuthClaims
        {
            UserReference = user.Reference
        });

        unitOfWork.Commit();

        return new LogInResponse
        {
            JwtToken = jwtToken
        };
    }
}