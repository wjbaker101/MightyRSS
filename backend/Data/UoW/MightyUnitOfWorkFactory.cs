namespace MightyRSS.Data.UoW;

public interface IUnitOfWorkFactory<out T>
{
    T Create();
}

public sealed class MightyUnitOfWorkFactory : IUnitOfWorkFactory<IMightyUnitOfWork>
{
    private readonly IApiDatabase _database;

    public MightyUnitOfWorkFactory(IApiDatabase database)
    {
        _database = database;
    }

    public IMightyUnitOfWork Create()
    {
        return new MightyUnitOfWork(_database);
    }
}