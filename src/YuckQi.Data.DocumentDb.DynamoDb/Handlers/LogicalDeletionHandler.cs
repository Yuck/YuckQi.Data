using System;
using Amazon.DynamoDBv2.DataModel;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Handlers.Abstract.Interfaces;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.DocumentDb.DynamoDb.Handlers;

public class LogicalDeletionHandler<TEntity, TIdentifier, TScope> : LogicalDeletionHandlerBase<TEntity, TIdentifier, TScope?> where TEntity : IEntity<TIdentifier>, IDeleted, IRevised where TIdentifier : IEquatable<TIdentifier> where TScope : IDynamoDBContext?
{
    public LogicalDeletionHandler(IRevisionHandler<TEntity, TIdentifier, TScope?> reviser) : base(reviser) { }
}
