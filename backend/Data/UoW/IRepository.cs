using NHibernate;
using System.Diagnostics.CodeAnalysis;

namespace Data.UoW;

public interface IRepository<TRecord>
{
    TRecord Save(TRecord record);
    IEnumerable<TRecord> SaveMany(IEnumerable<TRecord> records);
    TRecord Update(TRecord record);
    IEnumerable<TRecord> UpdateMany(IEnumerable<TRecord> records);
    void Delete(TRecord record);
    void DeleteMany(IEnumerable<TRecord> records);
}

public abstract class Repository<TRecord>
{
    protected readonly ISession Session;

    protected Repository(ISession session)
    {
        Session = session;
    }

    public TRecord Save(TRecord record)
    {
        Session.Save(record);
        return record;
    }

    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public IEnumerable<TRecord> SaveMany(IEnumerable<TRecord> records)
    {
        foreach (var record in records)
            Save(record);

        return records;
    }

    public TRecord Update(TRecord record)
    {
        Session.Update(record);
        return record;
    }

    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public IEnumerable<TRecord> UpdateMany(IEnumerable<TRecord> records)
    {
        foreach (var record in records)
            Update(record);

        return records;
    }

    public void Delete(TRecord record)
    {
        Session.Delete(record);
    }

    public void DeleteMany(IEnumerable<TRecord> records)
    {
        foreach (var record in records)
            Delete(record);
    }
}