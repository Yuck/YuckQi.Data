using System;
using System.Threading.Tasks;
using Dapper;
using Mapster;
using YuckQi.Data.Abstract;
using YuckQi.Data.Exceptions;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Data.Sql.Dapper.Providers.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.Providers
{
    public class PhysicalDeletionProvider<TEntity, TKey, TRecord> : DataProviderBase, IPhysicalDeletionProvider<TEntity, TKey> where TEntity : IEntity<TKey> where TKey : struct
    {
        #region Constructors

        public PhysicalDeletionProvider(IUnitOfWork context) : base(context) { }

        #endregion


        #region Public Methods

        public async Task<TEntity> DeleteAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (await Context.Db.DeleteAsync(entity.Adapt<TRecord>(), Context.Transaction) <= 0)
                throw new RecordDeleteException<TRecord, TKey>(entity.Key);

            Context.SaveChanges();

            return entity;
        }

        #endregion
    }
}