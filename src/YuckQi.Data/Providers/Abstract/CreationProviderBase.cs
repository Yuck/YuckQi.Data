using System;
using System.Threading.Tasks;
using YuckQi.Data.Exceptions;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Providers.Abstract
{
    public abstract class CreationProviderBase<TEntity, TKey, TScope, TRecord> : ICreationProvider<TEntity, TKey, TScope> where TEntity : IEntity<TKey>, ICreated where TKey : struct
    {
        #region Public Methods

        public TEntity Create(TEntity entity, TScope scope)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            var key = DoCreate(entity, scope);
            if (key == null)
                throw new RecordInsertException<TRecord>();

            entity.Key = key.Value;

            return entity;
        }

        public async Task<TEntity> CreateAsync(TEntity entity, TScope scope)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            var key = await DoCreateAsync(entity, scope);
            if (key == null)
                throw new RecordInsertException<TRecord>();

            entity.Key = key.Value;

            return entity;
        }

        #endregion


        #region Protected Methods

        protected abstract TKey? DoCreate(TEntity entity, TScope scope);

        protected abstract Task<TKey?> DoCreateAsync(TEntity entity, TScope scope);

        #endregion
    }
}
