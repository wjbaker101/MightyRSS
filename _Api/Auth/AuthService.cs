using MightyRSS._Api.Auth.Types;
using MightyRSS.Auth;
using MightyRSS.Auth.Types;
using MightyRSS.Data.Records;
using MightyRSS.Data.UoW;
using System;
using WJBCommon.Lib.Data;

namespace MightyRSS._Api.Auth
{
    public interface IAuthService
    {
        GetUserResponse GetUser(Guid reference);
        CreateUserResponse CreateUser(CreateUserRequest request);
        LogInResponse LogIn(LogInRequest request);
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

        public GetUserResponse GetUser(Guid reference)
        {
            using var unitOfWork = _mightyUnitOfWork.Create();

            var user = unitOfWork.Users.GetByReference(reference);
            if (user == null)
                return null;

            unitOfWork.Commit();

            return new GetUserResponse
            {
                Reference = user.Reference,
                Username = user.Username
            };
        }

        public CreateUserResponse CreateUser(CreateUserRequest request)
        {
            var userReference = Guid.NewGuid();
            var passwordSalt = Guid.NewGuid();
            var hashedPassword = _passwordHelper.HashPassword(request.Password, passwordSalt);

            using var unitOfWork = _mightyUnitOfWork.Create();

            var user = new UserRecord
            {
                Reference = userReference,
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

        public LogInResponse LogIn(LogInRequest request)
        {
            using var unitOfWork = _mightyUnitOfWork.Create();

            var user = unitOfWork.Users.GetByUsername(request.Username);
            if (user == null)
                return null;
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
}