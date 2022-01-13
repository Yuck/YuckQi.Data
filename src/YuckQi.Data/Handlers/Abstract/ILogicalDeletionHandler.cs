﻿using System.Threading.Tasks;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Handlers.Abstract
{
    public interface ILogicalDeletionHandler<TEntity, in TKey, in TScope> where TEntity : IEntity<TKey>, IDeleted, IRevised where TKey : struct
    {
        TEntity Delete(TEntity entity, TScope scope);

        Task<TEntity> DeleteAsync(TEntity entity, TScope scope);

        TEntity Restore(TEntity entity, TScope scope);

        Task<TEntity> RestoreAsync(TEntity entity, TScope scope);
    }
}