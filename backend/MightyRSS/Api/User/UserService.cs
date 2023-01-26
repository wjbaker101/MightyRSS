using Core.Models.Mappers;
using Data.UoW;
using MightyRSS.Api.User.Types;
using MightyRSS.Types;
using NetApiLibs.Type;

namespace MightyRSS.Api.User;

public interface IUserService
{
    Result<GetSelfResponse> GetSelf(IRequestContext requestContext);
}

public sealed class UserService : IUserService
{
    private readonly IUnitOfWorkFactory<IMightyUnitOfWork> _mightyUnitOfWorkFactory;

    public UserService(IUnitOfWorkFactory<IMightyUnitOfWork> mightyUnitOfWorkFactory)
    {
        _mightyUnitOfWorkFactory = mightyUnitOfWorkFactory;
    }

    public Result<GetSelfResponse> GetSelf(IRequestContext requestContext)
    {
        var self = requestContext.User;

        return new GetSelfResponse
        {
            User = UserMapper.Map(self)
        };
    }
}