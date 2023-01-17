using Core.Settings;
using Data;
using Data.UoW;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MightyRSS._Api.Auth;
using MightyRSS._Api.Feed;
using MightyRSS.Auth;

namespace MightyRSS;

public sealed class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IWebHostEnvironment env)
    {
        var appSettingsBaseFileName = env.IsDevelopment() ? "appsettings.Development" : "appsettings";

        Configuration = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile($"{appSettingsBaseFileName}.json")
            .AddJsonFile($"{appSettingsBaseFileName}.Secrets.json")
            .Build();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<DatabaseSettings>(Configuration.GetSection("Database"));
        services.Configure<FeedSettings>(Configuration.GetSection("Feed"));

        services.AddScoped<IRequestContext, RequestContext>();
        services.AddScoped<Authorisation>();

        services.AddSingleton<IApiDatabase, ApiDatabase>();
        services.AddSingleton<IUnitOfWorkFactory<IMightyUnitOfWork>, MightyUnitOfWorkFactory>();

        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<IPasswordHelper, PasswordHelper>();
        services.AddSingleton<ILoginTokenService, LoginTokenService>();

        services.AddSingleton<IFeedReaderService, FeedReaderService>();
        services.AddSingleton<IFeedService, FeedService>();

        services.AddControllers();

        services.AddSpaStaticFiles(configuration =>
        {
            configuration.RootPath = "wwwroot";
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseSpa(spa =>
        {
            spa.Options.SourcePath = "wwwroot";
        });

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}