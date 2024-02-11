using MightyRSS.BackgroundServices;

namespace MightyRSS.Setup;

public static class SetupHostedServices
{
    public static void AddHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<FeedBackgroundService>();
    }
}