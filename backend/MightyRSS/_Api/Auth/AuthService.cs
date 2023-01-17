using Data.Records;
using Data.UoW;
using MightyRSS._Api.Auth.Types;
using MightyRSS.Auth.Types;
using NetApiLibs.Type;
using System;

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
    private readonly IPasswordService _passwordService;
    private readonly ILoginTokenService _loginTokenService;

    public AuthService(
        IUnitOfWorkFactory<IMightyUnitOfWork> mightyUnitOfWork,
        IPasswordService passwordService,
        ILoginTokenService loginTokenService)
    {
        _mightyUnitOfWork = mightyUnitOfWork;
        _passwordService = passwordService;
        _loginTokenService = loginTokenService;
    }

    public Result<GetUserResponse> GetUser(Guid reference)
    {
        using var unitOfWork = _mightyUnitOfWork.Create();

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

        var userResult = unitOfWork.Users.GetByUsername(request.Username);
        if (!userResult.TrySuccess(out var user))
            return Result<LogInResponse>.FromFailure(userResult);

        if (!_passwordService.IsMatch(user.Password, request.Password, user.PasswordSalt))
            return null;

        var jwtToken = _loginTokenService.CreateToken(new AuthClaims
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