using NHibernate;

namespace Data.UoW;

public interface IRepository<TRecord>
{
    Task<TRecord> Save(TRecord record);
    Task<List<TRecord>> SaveMany(IEnumerable<TRecord> records);
    Task<TRecord> Update(TRecord record);
    Task<List<TRecord>> UpdateMany(IEnumerable<TRecord> records);
    Task Delete(TRecord record);
    Task DeleteMany(IEnumerable<TRecord> records);
}

public abstract class Repository<TRecord>
{
    protected readonly ISession Session;
    protected readonly CancellationToken CancellationToken;

    protected Repository(ISession session, CancellationToken cancellationToken)
    {
        Session = session;
        CancellationToken = cancellationToken;
    }

    public async Task<TRecord> Save(TRecord record)
    {
        await Session.SaveAsync(record, CancellationToken);
        return record;
    }

    public async Task<List<TRecord>> SaveMany(IEnumerable<TRecord> records)
    {
        var toSave = records.ToList();

        foreach (var record in toSave)
            await Save(record);

        return toSave;
    }

    public async Task<TRecord> Update(TRecord record)
    {
        await Session.UpdateAsync(record, CancellationToken);
        return record;
    }

    public async Task<List<TRecord>> UpdateMany(IEnumerable<TRecord> records)
    {
        var toUpdate = records.ToList();

        foreach (var record in toUpdate)
            await Update(record);

        return toUpdate;
    }

    public async Task Delete(TRecord record)
    {
        await Session.DeleteAsync(record, CancellationToken);
    }

    public async Task DeleteMany(IEnumerable<TRecord> records)
    {
        foreach (var record in records)
            await Delete(record);
    }
}