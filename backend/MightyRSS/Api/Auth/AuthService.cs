using Data.UoW;
using MightyRSS.Api.Auth.Types;
using NetApiLibs.Type;
using System.Threading;
using System.Threading.Tasks;

namespace MightyRSS.Api.Auth;

public interface IAuthService
{
    Task<Result<LogInResponse>> LogIn(LogInRequest request, CancellationToken cancellationToken);
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

    public async Task<Result<LogInResponse>> LogIn(LogInRequest request, CancellationToken cancellationToken)
    {
        using var unitOfWork = _mightyUnitOfWork.Create(cancellationToken);

        var userResult = await unitOfWork.Users.GetByUsername(request.Username);
        if (!userResult.TrySuccess(out var user))
            return Result<LogInResponse>.FromFailure(userResult);

        if (!_passwordService.IsMatch(user.Password, request.Password, user.PasswordSalt))
            return Result<LogInResponse>.Failure("Incorrect password, please try again.");

        var jwtToken = _loginTokenService.CreateToken(user.Reference);

        await unitOfWork.Commit();

        return new LogInResponse
        {
            JwtToken = jwtToken
        };
    }
}