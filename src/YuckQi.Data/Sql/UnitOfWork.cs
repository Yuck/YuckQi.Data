using System.Data;
using YuckQi.Data.Abstract.Interfaces;

namespace YuckQi.Data.Sql;

public class UnitOfWork<TScope, TDbConnection> : IUnitOfWork<TScope> where TScope : class, IDbTransaction where TDbConnection : class, IDbConnection
{
    private TDbConnection? _connection;
    private readonly IsolationLevel _isolation;
    private readonly Object _lock = new ();
    private Lazy<TScope>? _transaction;

    public TScope Scope => _transaction != null ? _transaction.Value : throw new NullReferenceException();

    public UnitOfWork(TDbConnection connection, IsolationLevel isolation = IsolationLevel.ReadCommitted)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _isolation = isolation;
        _transaction = new Lazy<TScope>(StartTransaction);
    }

    public void Dispose()
    {
        if (_transaction != null)
        {
            Scope.Rollback();
            Scope.Dispose();

            _transaction = null;
        }

        if (_connection == null)
            return;

        _connection?.Close();
        _connection?.Dispose();

        _connection = null;
    }

    public void SaveChanges()
    {
        lock (_lock)
        {
            if (_transaction == null)
                throw new InvalidOperationException();

            Scope.Commit();
            Scope.Dispose();

            _transaction = new Lazy<TScope>(StartTransaction);
        }
    }

    private TScope StartTransaction()
    {
        lock (_lock)
        {
            if (_connection is { State: ConnectionState.Closed })
                _connection.Open();

            return _connection != null ? (TScope) _connection.BeginTransaction(_isolation) : throw new NullReferenceException();
        }
    }
}
