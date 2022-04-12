using System;
using System.Threading;
using Amazon.DynamoDBv2;
using YuckQi.Data.Abstract;

namespace YuckQi.Data.DocumentDb.DynamoDb
{
    public class UnitOfWork : IUnitOfWork<IAmazonDynamoDB>
    {
        #region Private Members

        #endregion


        #region Properties

        public IAmazonDynamoDB Scope { get; private set; }

        #endregion


        #region Constructors

        public UnitOfWork(IAmazonDynamoDB client)
        {
            Scope = client ?? throw new ArgumentNullException(nameof(client));
        }

        #endregion


        #region Public Methods

        public void Dispose()
        {
            if (Scope == null)
                return;

            Scope?.Dispose();

            Scope = null;
        }

        public void SaveChanges(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
