using System;
using System.Data;
using YuckQi.Data.Abstract;

namespace YuckQi.Data.Sql;

public class UnitOfWork<TScope, TDbConnection> : IUnitOfWork<TScope> where TScope : class, IDbTransaction where TDbConnection : class, IDbConnection
{
    #region Private Members

    private TDbConnection _connection;
    private readonly IsolationLevel _isolation;
    private readonly Object _lock = new();
    private Lazy<TScope> _transaction;

    #endregion


    #region Properties

    public TScope Scope => _transaction.Value;

    #endregion


    #region Constructors

    public UnitOfWork(TDbConnection connection, IsolationLevel isolation = IsolationLevel.ReadCommitted)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _isolation = isolation;
        _transaction = new Lazy<TScope>(StartTransaction);
    }

    #endregion


    #region Public Methods

    public void Dispose()
    {
        if (_transaction != null)
        {
            Scope?.Rollback();
            Scope?.Dispose();

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

    #endregion


    #region Supporting Methods

    private TScope StartTransaction()
    {
        lock (_lock)
        {
            if (_connection.State == ConnectionState.Closed)
                _connection.Open();

            return _connection.BeginTransaction(_isolation) as TScope;
        }
    }

    #endregion
}
