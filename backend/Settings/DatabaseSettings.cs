namespace MightyRSS.Settings;

public sealed class DatabaseSettings
{
    public string Host { get; init; }
    public int Port { get; init; }
    public string Database { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }
}