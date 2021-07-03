using System;
using System.Data;
using YuckQi.Data.Abstract;

namespace YuckQi.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Private Members

        private readonly Object _lock = new Object();
        private Lazy<IDbTransaction> _transaction;

        #endregion


        #region Properties

        public IDbConnection Db { get; private set; }
        public IDbTransaction Transaction => _transaction.Value;

        #endregion


        #region Constructors

        public UnitOfWork(IDbConnection connection, IsolationLevel isolation = IsolationLevel.ReadCommitted)
        {
            _transaction = new Lazy<IDbTransaction>(() =>
            {
                lock (_lock)
                {
                    if (Db.State == ConnectionState.Closed)
                        Db.Open();

                    return Db.BeginTransaction(isolation);
                }
            });

            Db = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        #endregion


        #region Public Methods

        public void Dispose()
        {
            if (_transaction != null)
            {
                Transaction?.Rollback();
                Transaction?.Dispose();

                _transaction = null;
            }

            if (Db == null)
                return;

            Db?.Close();
            Db?.Dispose();

            Db = null;
        }

        public void SaveChanges()
        {
            lock (_lock)
            {
                if (_transaction == null)
                    throw new InvalidOperationException();

                Transaction.Commit();
                Transaction.Dispose();

                _transaction = null;
            }
        }

        #endregion
    }
}