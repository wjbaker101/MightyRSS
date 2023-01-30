using Data.UoW;
using MightyRSS.Api.Auth.Types;
using NetApiLibs.Type;

namespace MightyRSS.Api.Auth;

public interface IAuthService
{
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

    public Result<LogInResponse> LogIn(LogInRequest request)
    {
        using var unitOfWork = _mightyUnitOfWork.Create();

        var userResult = unitOfWork.Users.GetByUsername(request.Username);
        if (!userResult.TrySuccess(out var user))
            return Result<LogInResponse>.FromFailure(userResult);

        if (!_passwordService.IsMatch(user.Password, request.Password, user.PasswordSalt))
            return Result<LogInResponse>.Failure("Incorrect password, please try again.");

        var jwtToken = _loginTokenService.CreateToken(user.Reference);

        unitOfWork.Commit();

        return new LogInResponse
        {
            JwtToken = jwtToken
        };
    }
}