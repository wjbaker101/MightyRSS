using NHibernate;
using System;
using System.Data;

namespace MightyRSS.Data.UoW;

public interface IUnitOfWork : IDisposable
{
    void Commit();
}

public abstract class UnitOfWork : IUnitOfWork
{
    protected ISession Session { get; }
    protected ITransaction Transaction { get; }

    private bool _isDisposed;
    private bool _isSaved;

    protected UnitOfWork(IApiDatabase database)
    {
        Session = database.SessionFactory().OpenSession();
        Transaction = Session.BeginTransaction(IsolationLevel.ReadCommitted);

        _isDisposed = false;
        _isSaved = false;
    }

    public void Commit()
    {
        if (_isSaved)
            throw new InvalidOperationException("Unable to save the transaction since it has already been saved.");

        Transaction.Commit();

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