using MightyRSS.Data.Records;

namespace MightyRSS.Auth
{
    public interface IRequestContext
    {
        public UserRecord User { get; set; }
    }

    public sealed class RequestContext : IRequestContext
    {
        public UserRecord User { get; set; }
    }
}