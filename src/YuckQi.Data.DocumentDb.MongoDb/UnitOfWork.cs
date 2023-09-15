using MongoDB.Driver;
using YuckQi.Data.Abstract;

namespace YuckQi.Data.DocumentDb.MongoDb;

public class UnitOfWork : IUnitOfWork<IClientSessionHandle>
{
    private readonly IMongoClient _client;
    private readonly Object _lock = new ();
    private readonly ClientSessionOptions? _options;
    private Lazy<IClientSessionHandle>? _session;

    public IClientSessionHandle Scope => _session != null ? _session.Value : throw new NullReferenceException();

    public UnitOfWork(IMongoClient client, ClientSessionOptions? options = null)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _options = options;
        _session = new Lazy<IClientSessionHandle>(() => _client.StartSession(_options));
    }

    public void Dispose()
    {
        if (_session == null)
            return;

        if (Scope.IsInTransaction)
            Scope.AbortTransaction();

        Scope.Dispose();

        _session = null;
    }

    public void SaveChanges()
    {
        lock (_lock)
        {
            if (_session == null)
                throw new InvalidOperationException();

            if (Scope.IsInTransaction)
                Scope.CommitTransaction();

            _session = new Lazy<IClientSessionHandle>(() => _client.StartSession(_options));
        }
    }
}
