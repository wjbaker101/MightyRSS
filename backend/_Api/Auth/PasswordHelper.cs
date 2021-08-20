using System;
using System.Security.Cryptography;
using System.Text;

namespace MightyRSS._Api.Auth
{
    public interface IPasswordHelper
    {
        string HashPassword(string password, Guid salt);
        bool IsMatch(string expectedPassword, string password, Guid salt);
    }

    public sealed class PasswordHelper : IPasswordHelper
    {
        private readonly Guid _pepper;

        public PasswordHelper()
        {
            _pepper = Guid.Parse("523b683b-df54-4deb-a9f6-c0e0e99654b1");
        }

        public string HashPassword(string password, Guid salt)
        {
            var toHash = password + salt + _pepper;

            return Convert.ToBase64String(new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(toHash)));
        }

        public bool IsMatch(string expectedPassword, string password, Guid salt)
        {
            var hashed = HashPassword(password, salt);

            return hashed == expectedPassword;
        }
    }
}