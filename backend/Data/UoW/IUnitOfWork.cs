using NHibernate;
using System.Data;

namespace Data.UoW;

public interface IUnitOfWork : IDisposable
{
    Task Commit();
}

public abstract class UnitOfWork : IUnitOfWork
{
    protected ISession Session { get; }
    protected ITransaction Transaction { get; }

    private readonly CancellationToken _cancellationToken;

    private bool _isDisposed;
    private bool _isSaved;

    protected UnitOfWork(IApiDatabase database, CancellationToken cancellationToken)
    {
        Session = database.SessionFactory().OpenSession();
        Transaction = Session.BeginTransaction(IsolationLevel.ReadCommitted);

        _cancellationToken = cancellationToken;

        _isDisposed = false;
        _isSaved = false;
    }

    public async Task Commit()
    {
        if (_isSaved)
            throw new InvalidOperationException("Unable to save the transaction since it has already been saved.");

        await Transaction.CommitAsync(_cancellationToken);

        _isSaved = true;
    }

    public void Dispose()
    {
        if (_isDisposed)
            throw new ObjectDisposedException(nameof(UnitOfWork));

        if (!_isSaved)
            Transaction.Rollback();

        Transaction.Dispose();
        Session.Dispose();

        _isDisposed = true;
    }
}