﻿using MongoDB.Driver;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.DocumentDb.MongoDb.Handlers
{
    public class LogicalDeletionHandler<TEntity, TKey, TScope> : LogicalDeletionHandlerBase<TEntity, TKey, TScope> where TEntity : IEntity<TKey>, IDeleted, IRevised where TKey : struct where TScope : IClientSessionHandle
    {
        public LogicalDeletionHandler(IRevisionHandler<TEntity, TKey, TScope> reviser) : base(reviser) { }
    }
}