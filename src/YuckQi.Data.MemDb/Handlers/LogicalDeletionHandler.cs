using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Handlers.Abstract.Interfaces;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.MemDb.Handlers;

public class LogicalDeletionHandler<TEntity, TIdentifier, TScope> : LogicalDeletionHandlerBase<TEntity, TIdentifier, TScope?> where TEntity : IEntity<TIdentifier>, IDeleted, IRevised where TIdentifier : IEquatable<TIdentifier>
{
    public LogicalDeletionHandler(IRevisionHandler<TEntity, TIdentifier, TScope?> reviser) : base(reviser) { }
}
