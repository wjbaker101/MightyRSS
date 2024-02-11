using Data.Records;
using Data.UoW;
using MightyRSS.Api.Auth;
using MightyRSS.Api.User.Types;
using MightyRSS.Models.Mappers;
using MightyRSS.Types;
using NetApiLibs.Type;

namespace MightyRSS.Api.User;

public interface IUserService
{
    Result<GetSelfResponse> GetSelf(IRequestContext requestContext);
    Task<Result<GetUserResponse>> GetUser(Guid reference, CancellationToken cancellationToken);
    Task<Result<CreateUserResponse>> CreateUser(CreateUserRequest request, CancellationToken cancellationToken);
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

    public async Task<Result<GetUserResponse>> GetUser(Guid reference, CancellationToken cancellationToken)
    {
        using var unitOfWork = _mightyUnitOfWorkFactory.Create(cancellationToken);

        var userResult = await unitOfWork.Users.GetByReference(reference);
        if (!userResult.TrySuccess(out var user))
            return Result<GetUserResponse>.FromFailure(userResult);

        await unitOfWork.Commit();

        return new GetUserResponse
        {
            Reference = user.Reference,
            Username = user.Username
        };
    }

    public async Task<Result<CreateUserResponse>> CreateUser(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var userReference = Guid.NewGuid();
        var passwordSalt = Guid.NewGuid();
        var hashedPassword = _passwordService.HashPassword(request.Password, passwordSalt);

        using var unitOfWork = _mightyUnitOfWorkFactory.Create(cancellationToken);

        var user = new UserRecord
        {
            Reference = userReference,
            CreatedAt = DateTime.UtcNow,
            Username = request.Username,
            Password = hashedPassword,
            PasswordSalt = passwordSalt
        };

        await unitOfWork.Users.Save(user);

        await unitOfWork.Commit();

        return new CreateUserResponse
        {
            Reference = user.Reference,
            Username = user.Username
        };
    }
}