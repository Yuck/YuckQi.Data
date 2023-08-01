using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Handlers.Abstract;

public interface IPhysicalDeletionHandler<TEntity, in TIdentifier, in TScope> where TEntity : IEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier>
{
    TEntity Delete(TEntity entity, TScope? scope);

    Task<TEntity> Delete(TEntity entity, TScope? scope, CancellationToken cancellationToken);
}
