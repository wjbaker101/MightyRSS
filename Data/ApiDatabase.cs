using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.Options;
using MightyRSS.Settings;
using NHibernate;

namespace MightyRSS.Data
{
    public interface IApiDatabase
    {
        ISessionFactory SessionFactory { get; }
    }

    public sealed class ApiDatabase: IApiDatabase
    {
        public ISessionFactory SessionFactory { get; }

        public ApiDatabase(IOptions<DatabaseSettings> databaseSettings)
        {
            SessionFactory = CreateSessionFactory(databaseSettings.Value);
        }

        private ISessionFactory CreateSessionFactory(DatabaseSettings databaseSettings)
        {
            return Fluently.Configure()
                .Database(PostgreSQLConfiguration.Standard.ConnectionString(c => c
                    .Host(databaseSettings.Host)
                    .Port(databaseSettings.Port)
                    .Database(databaseSettings.Database)
                    .Username(databaseSettings.Username)
                    .Password(databaseSettings.Password)))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Program>())
                .BuildSessionFactory();
        }
    }
}