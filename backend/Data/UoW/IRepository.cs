using NHibernate;
using System.Collections.Generic;

namespace MightyRSS.Data.UoW;

public interface IRepository<in TRecord>
{
    void Save(TRecord record);
    void SaveMany(IEnumerable<TRecord> records);
    void Update(TRecord record);
    void UpdateMany(IEnumerable<TRecord> records);
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

    public void Save(TRecord record)
    {
        Session.Save(record);
    }

    public void SaveMany(IEnumerable<TRecord> records)
    {
        foreach (var record in records)
            Save(record);
    }

    public void Update(TRecord record)
    {
        Session.Update(record);
    }

    public void UpdateMany(IEnumerable<TRecord> records)
    {
        foreach (var record in records)
            Update(record);
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