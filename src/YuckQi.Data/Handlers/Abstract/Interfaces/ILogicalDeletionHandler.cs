using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Handlers.Abstract.Interfaces;

public interface ILogicalDeletionHandler<TEntity, in TIdentifier, in TScope> where TEntity : IEntity<TIdentifier>, IDeleted, IRevised where TIdentifier : IEquatable<TIdentifier>
{
    TEntity Delete(TEntity entity, TScope? scope);

    Task<TEntity> Delete(TEntity entity, TScope? scope, CancellationToken cancellationToken);

    TEntity Restore(TEntity entity, TScope? scope);

    Task<TEntity> Restore(TEntity entity, TScope? scope, CancellationToken cancellationToken);
}
