﻿using System;
using System.Threading.Tasks;
using YuckQi.Data.Exceptions;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Providers.Abstract
{
    public abstract class PhysicalDeletionProviderBase<TEntity, TKey, TScope, TRecord> : IPhysicalDeletionProvider<TEntity, TKey, TScope> where TEntity : IEntity<TKey> where TKey : struct
    {
        #region Public Methods

        public TEntity Delete(TEntity entity, TScope scope)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            if (! DoDelete(entity, scope))
                throw new RecordDeleteException<TRecord, TKey>(entity.Key);

            return entity;
        }

        public async Task<TEntity> DeleteAsync(TEntity entity, TScope scope)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            if (! await DoDeleteAsync(entity, scope))
                throw new RecordDeleteException<TRecord, TKey>(entity.Key);

            return entity;
        }

        #endregion


        #region Protected Methods

        protected abstract Boolean DoDelete(TEntity entity, TScope scope);

        protected abstract Task<Boolean> DoDeleteAsync(TEntity entity, TScope scope);

        #endregion
    }
}