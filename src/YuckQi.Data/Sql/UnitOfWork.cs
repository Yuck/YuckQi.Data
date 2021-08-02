using System;
using System.Data;
using YuckQi.Data.Abstract;

namespace YuckQi.Data.Sql
{
    public class UnitOfWork : IUnitOfWork<IDbTransaction>
    {
        #region Private Members

        private IDbConnection _connection;
        private readonly Object _lock = new Object();
        private Lazy<IDbTransaction> _transaction;

        #endregion


        #region Properties

        public IDbTransaction Scope => _transaction.Value;

        #endregion


        #region Constructors

        public UnitOfWork(IDbConnection connection, IsolationLevel isolation = IsolationLevel.ReadCommitted)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _transaction = new Lazy<IDbTransaction>(() =>
            {
                lock (_lock)
                {
                    if (_connection.State == ConnectionState.Closed)
                        _connection.Open();

                    return _connection.BeginTransaction(isolation);
                }
            });
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

                _transaction = null;
            }
        }

        #endregion
    }
}