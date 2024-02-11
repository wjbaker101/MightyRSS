using Data;
using Data.UoW;
using MightyRSS.Api.Auth;
using MightyRSS.Api.Auth.Attributes;
using MightyRSS.Api.Collections;
using MightyRSS.Api.Feed;
using MightyRSS.Api.FeedSources;
using MightyRSS.Api.User;
using MightyRSS.Types;

namespace MightyRSS.Setup;

public static class SetupDependencies
{
    public static void AddDependencies(this IServiceCollection services)
    {
        services.AddScoped<IRequestContext, RequestContext>();
        services.AddScoped<Authorisation>();

        services.AddSingleton<IApiDatabase, ApiDatabase>();
        services.AddSingleton<IUnitOfWorkFactory<IMightyUnitOfWork>, MightyUnitOfWorkFactory>();

        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<IPasswordService, PasswordService>();
        services.AddSingleton<ILoginTokenService, LoginTokenService>();

        services.AddSingleton<ICollectionsService, CollectionsService>();

        services.AddSingleton<IFeedService, FeedService>();

        services.AddSingleton<IFeedSourcesService, FeedSourcesService>();
        services.AddSingleton<IFeedReaderService, FeedReaderService>();

        services.AddSingleton<IUserService, UserService>();
    }
}