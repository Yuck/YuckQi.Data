using System;
using System.Threading.Tasks;
using YuckQi.Data.Exceptions;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Providers.Abstract
{
    public abstract class RevisionProviderBase<TEntity, TKey, TScope, TRecord> : IRevisionProvider<TEntity, TKey, TScope> where TEntity : IEntity<TKey>, IRevised where TKey : struct
    {
        #region Public Methods

        public TEntity Revise(TEntity entity, TScope scope)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            if (entity.RevisionMomentUtc == DateTime.MinValue)
                entity.RevisionMomentUtc = DateTime.UtcNow;

            if (! DoRevise(entity, scope))
                throw new RecordUpdateException<TRecord, TKey>(entity.Key);

            return entity;
        }

        public async Task<TEntity> ReviseAsync(TEntity entity, TScope scope)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            if (entity.RevisionMomentUtc == DateTime.MinValue)
                entity.RevisionMomentUtc = DateTime.UtcNow;

            if (! await DoReviseAsync(entity, scope))
                throw new RecordUpdateException<TRecord, TKey>(entity.Key);

            return entity;
        }

        #endregion


        #region Protected Methods

        protected abstract Boolean DoRevise(TEntity entity, TScope scope);

        protected abstract Task<Boolean> DoReviseAsync(TEntity entity, TScope scope);

        #endregion
    }
}
