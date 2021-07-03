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
    public class CreationProvider<TEntity, TKey, TRecord> : DataProviderBase, ICreationProvider<TEntity, TKey> where TEntity : IEntity<TKey>, ICreated where TKey : struct
    {
        #region Constructors

        public CreationProvider(IUnitOfWork context) : base(context) { }

        #endregion


        #region Public Methods

        public TEntity Create(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.CreationMomentUtc = DateTime.UtcNow;

            if (! (Context.Db.Insert(entity.Adapt<TRecord>(), Context.Transaction) > 0))
                throw new RecordInsertException<TKey>();

            return entity;
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.CreationMomentUtc = DateTime.UtcNow;

            if (! (await Context.Db.InsertAsync(entity.Adapt<TRecord>(), Context.Transaction) > 0))
                throw new RecordInsertException<TKey>();

            return entity;
        }

        #endregion
    }
}