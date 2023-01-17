using Core.Settings;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.Options;
using NHibernate;

namespace Data;

public interface IApiDatabase
{
    ISessionFactory SessionFactory();
}

public sealed class ApiDatabase : IApiDatabase
{
    private readonly ISessionFactory _sessionFactory;
        
    public ApiDatabase(IOptions<DatabaseSettings> databaseSettings)
    {
        _sessionFactory = CreateSessionFactory(databaseSettings.Value);
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
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ApiDatabase>())
            .BuildSessionFactory();
    }

    public ISessionFactory SessionFactory()
    {
        return _sessionFactory;
    }
}