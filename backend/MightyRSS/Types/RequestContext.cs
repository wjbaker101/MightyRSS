using Data.Records;

namespace MightyRSS.Types;

public interface IRequestContext
{
    public UserRecord User { get; set; }
}

public sealed class RequestContext : IRequestContext
{
    public required UserRecord User { get; set; }
}