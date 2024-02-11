using Core.Settings;

namespace MightyRSS.Setup;

public static class SetupSettings
{
    public static void AddSettings(this WebApplicationBuilder builder)
    {
        var services = builder.Services;

        var appSettingsBaseFileName = builder.Environment.IsDevelopment() ? "appsettings.Development" : "appsettings";

        builder.Configuration
            .SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile($"{appSettingsBaseFileName}.json")
            .AddJsonFile($"{appSettingsBaseFileName}.Secrets.json")
            .Build();

        services.Configure<DatabaseSettings>(builder.Configuration.GetSection("Database"));
        services.Configure<FeedSettings>(builder.Configuration.GetSection("Feed"));
    }
}