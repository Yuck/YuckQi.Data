using System;
using System.Threading.Tasks;
using Dapper;
using Mapster;
using YuckQi.Data.Abstract;
using YuckQi.Data.Entities.Abstract;
using YuckQi.Data.Exceptions;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Sql.Dapper.Handlers.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.Handlers
{
    public class CreationHandler<TEntity, TKey, TRecord> : DataHandlerBase, ICreationHandler<TEntity, TKey> where TEntity : IEntity<TKey>, ICreated where TKey : struct
    {
        #region Constructors

        public CreationHandler(IUnitOfWork uow) : base(uow)
        {
        }

        #endregion


        #region Public Methods

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.CreationMomentUtc = DateTime.UtcNow;

            if (await Db.InsertAsync(entity.Adapt<TRecord>(), Transaction) > 0)
                return entity;

            throw new RecordInsertException<TKey>();
        }

        #endregion
    }
}