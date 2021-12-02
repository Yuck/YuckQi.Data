using System;
using System.Threading.Tasks;
using YuckQi.Data.Exceptions;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Handlers.Abstract
{
    public abstract class PhysicalDeletionHandlerBase<TEntity, TKey, TScope, TRecord> : IPhysicalDeletionHandler<TEntity, TKey, TScope> where TEntity : IEntity<TKey> where TKey : struct
    {
        #region Properties

        protected IMapper Mapper { get; }

        #endregion


        #region Constructors

        protected PhysicalDeletionHandlerBase(IMapper mapper)
        {
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        #endregion


        #region Public Methods

        public TEntity Delete(TEntity entity, TScope scope)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            if (! DoDelete(entity, scope))
                throw new PhysicalDeletionException<TRecord, TKey>(entity.Key);

            return entity;
        }

        public async Task<TEntity> DeleteAsync(TEntity entity, TScope scope)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            if (! await DoDeleteAsync(entity, scope))
                throw new PhysicalDeletionException<TRecord, TKey>(entity.Key);

            return entity;
        }

        #endregion


        #region Protected Methods

        protected abstract Boolean DoDelete(TEntity entity, TScope scope);

        protected abstract Task<Boolean> DoDeleteAsync(TEntity entity, TScope scope);

        #endregion
    }
}
