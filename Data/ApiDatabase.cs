using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
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

        public ApiDatabase()
        {
            SessionFactory = CreateSessionFactory();
        }

        private ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database(PostgreSQLConfiguration.Standard.ConnectionString(c =>
                    c.Host("localhost")
                        .Port(5432)
                        .Database("mighty_rss")
                        .Username("postgres")
                        .Password("password")))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Program>())
                .BuildSessionFactory();
        }
    }
}