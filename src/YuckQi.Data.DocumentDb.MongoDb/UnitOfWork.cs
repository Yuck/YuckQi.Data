using System;
using MongoDB.Driver;
using YuckQi.Data.Abstract;

namespace YuckQi.Data.DocumentDb.MongoDb
{
    public class UnitOfWork : IUnitOfWork<IClientSessionHandle>
    {
        #region Private Members

        private readonly Object _lock = new Object();
        private Lazy<IClientSessionHandle> _session;

        #endregion


        #region Properties

        public IClientSessionHandle Scope => _session.Value;

        #endregion


        #region Constructors

        public UnitOfWork(IMongoClient client, ClientSessionOptions options = null)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            _session = new Lazy<IClientSessionHandle>(() => client.StartSession(options));
        }

        #endregion


        #region Public Methods

        public void Dispose()
        {
            if (_session == null)
                return;

            Scope?.AbortTransaction();
            Scope?.Dispose();

            _session = null;
        }

        public void SaveChanges()
        {
            lock (_lock)
            {
                if (_session == null)
                    throw new InvalidOperationException();

                Scope.CommitTransaction();
                Scope.Dispose();

                _session = null;
            }
        }

        #endregion
    }
}