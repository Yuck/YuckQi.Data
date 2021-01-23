﻿using System.Threading.Tasks;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Providers.Abstract
{
    public interface ILogicalDeletionProvider<TEntity, in TKey> where TEntity : IEntity<TKey>, IDeleted, IRevised where TKey : struct
    {
        Task<TEntity> DeleteAsync(TEntity entity);
        Task<TEntity> RestoreAsync(TEntity entity);
    }
}