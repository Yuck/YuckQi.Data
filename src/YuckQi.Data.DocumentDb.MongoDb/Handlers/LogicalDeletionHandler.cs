using MongoDB.Driver;
using YuckQi.Data.Handlers.Write.Abstract;
using YuckQi.Data.Handlers.Write.Abstract.Interfaces;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.DocumentDb.MongoDb.Handlers;

public class LogicalDeletionHandler<TEntity, TIdentifier, TScope> : LogicalDeletionHandlerBase<TEntity, TIdentifier, TScope?> where TEntity : IEntity<TIdentifier>, IDeleted, IRevised where TIdentifier : IEquatable<TIdentifier> where TScope : IClientSessionHandle?
{
    public LogicalDeletionHandler(IRevisionHandler<TEntity, TIdentifier, TScope?> reviser) : base(reviser) { }
}
