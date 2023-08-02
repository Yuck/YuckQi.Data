using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Handlers.Abstract;

public interface IActivationHandler<TEntity, in TIdentifier, in TScope> where TEntity : IEntity<TIdentifier>, IActivated, IRevised where TIdentifier : IEquatable<TIdentifier>
{
    TEntity Activate(TEntity entity, TScope? scope);

    Task<TEntity> Activate(TEntity entity, TScope? scope, CancellationToken cancellationToken);

    TEntity Deactivate(TEntity entity, TScope? scope);

    Task<TEntity> Deactivate(TEntity entity, TScope? scope, CancellationToken cancellationToken);
}
