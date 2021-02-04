using System;
using System.Threading.Tasks;
using Dapper;
using Mapster;
using YuckQi.Data.Abstract;
using YuckQi.Data.Exceptions;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Data.Sql.Dapper.Providers.Abstract;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.Providers
{
    public class RevisionProvider<TEntity, TKey, TRecord> : DataProviderBase, IRevisionProvider<TEntity, TKey> where TEntity : IEntity<TKey>, IRevised where TKey : struct
    {
        #region Constructors

        public RevisionProvider(IUnitOfWork context) : base(context)
        {
        }

        #endregion


        #region Public Methods

        public async Task<TEntity> ReviseAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.RevisionMomentUtc = DateTime.UtcNow;

            if (await Context.Db.UpdateAsync(entity.Adapt<TRecord>(), Context.Transaction) <= 0)
                throw new RecordUpdateException<TRecord, TKey>(entity.Key);

            Context.SaveChanges();

            return entity;
        }

        #endregion
    }
}