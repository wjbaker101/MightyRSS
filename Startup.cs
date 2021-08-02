using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MightyRSS._Api.Auth;
using MightyRSS._Api.Feed;
using MightyRSS._Api.Feed.Types;
using MightyRSS.Auth;
using MightyRSS.Data;
using MightyRSS.Data.Repositories;

namespace MightyRSS
{
    public sealed class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<FeedSettings>(Configuration.GetSection("Feed"));

            services.AddScoped<IRequestContext, RequestContext>();
            services.AddScoped<Authorisation>();
            services.AddSingleton<IJwtHelper, JwtHelper>();

            services.AddSingleton<IApiDatabase, ApiDatabase>();
            services.AddSingleton<IFeedSourceRepository, FeedSourceRepository>();
            services.AddSingleton<IUserDataFeedSourceRepository, UserDataFeedSourceRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();

            services.AddSingleton<IPasswordHelper, PasswordHelper>();
            services.AddSingleton<IAuthService, AuthService>();

            services.AddSingleton<IFeedReaderService, FeedReaderService>();
            services.AddSingleton<IFeedService, FeedService>();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}