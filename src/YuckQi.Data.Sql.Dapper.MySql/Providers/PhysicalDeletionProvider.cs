using System;
using System.Threading.Tasks;
using YuckQi.Data.Abstract;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Data.Sql.Dapper.Providers.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.MySql.Providers
{
    public class PhysicalDeletionProvider<TEntity, TKey, TRecord> : DataProviderBase, IPhysicalDeletionProvider<TEntity, TKey> where TEntity : IEntity<TKey> where TKey : struct
    {
        #region Constructors

        public PhysicalDeletionProvider(IUnitOfWork context) : base(context) { }

        #endregion


        #region Public Methods

        public Task<TEntity> DeleteAsync(TEntity entity) => throw new NotImplementedException();

        #endregion
    }
}