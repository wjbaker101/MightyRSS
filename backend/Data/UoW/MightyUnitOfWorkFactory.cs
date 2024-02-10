namespace Data.UoW;

public interface IUnitOfWorkFactory<out T>
{
    T Create(CancellationToken cancellationToken);
}

public sealed class MightyUnitOfWorkFactory : IUnitOfWorkFactory<IMightyUnitOfWork>
{
    private readonly IApiDatabase _database;

    public MightyUnitOfWorkFactory(IApiDatabase database)
    {
        _database = database;
    }

    public IMightyUnitOfWork Create(CancellationToken cancellationToken)
    {
        return new MightyUnitOfWork(_database, cancellationToken);
    }
}