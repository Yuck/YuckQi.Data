using System;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.DocumentDb.DynamoDb.Handlers;

public class LogicalDeletionHandler<TEntity, TIdentifier, TScope> : LogicalDeletionHandlerBase<TEntity, TIdentifier, TScope> where TEntity : IEntity<TIdentifier>, IDeleted, IRevised where TIdentifier : IEquatable<TIdentifier>
{
    public LogicalDeletionHandler(IRevisionHandler<TEntity, TIdentifier, TScope> reviser) : base(reviser) { }
}
