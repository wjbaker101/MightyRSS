using MightyRSS._Api.Auth.Types;
using MightyRSS.Data.Records;
using MightyRSS.Data.Repositories;
using System;
using MightyRSS.Auth;
using MightyRSS.Auth.Types;

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
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHelper _passwordHelper;
        private readonly IJwtHelper _jwtHelper;

        public AuthService(IUserRepository userRepository, IPasswordHelper passwordHelper, IJwtHelper jwtHelper)
        {
            _userRepository = userRepository;
            _passwordHelper = passwordHelper;
            _jwtHelper = jwtHelper;
        }

        public GetUserResponse GetUser(Guid reference)
        {
            var user = _userRepository.GetByReference(reference);
            if (user == null)
                return null;

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

            var user = _userRepository.Save(new UserRecord
            {
                Reference = userReference,
                Username = request.Username,
                Password = hashedPassword,
                PasswordSalt = passwordSalt
            });

            return new CreateUserResponse
            {
                Reference = user.Reference,
                Username = user.Username
            };
        }

        public LogInResponse LogIn(LogInRequest request)
        {
            var user = _userRepository.GetByUsername(request.Username);
            if (user == null)
                return null;
            if (!_passwordHelper.IsMatch(user.Password, request.Password, user.PasswordSalt))
                return null;

            var jwtToken = _jwtHelper.CreateToken(new AuthClaims
            {
                UserReference = user.Reference
            });

            return new LogInResponse
            {
                JwtToken = jwtToken
            };
        }
    }
}