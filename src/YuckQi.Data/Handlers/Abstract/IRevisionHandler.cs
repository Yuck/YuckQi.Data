using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Handlers.Abstract;

public interface IRevisionHandler<TEntity, TIdentifier, in TScope> where TEntity : IEntity<TIdentifier>, IRevised where TIdentifier : IEquatable<TIdentifier>
{
    TEntity Revise(TEntity entity, TScope? scope);

    IEnumerable<TEntity> Revise(IEnumerable<TEntity> entities, TScope? scope);

    Task<TEntity> Revise(TEntity entity, TScope? scope, CancellationToken cancellationToken);

    Task<IEnumerable<TEntity>> Revise(IEnumerable<TEntity> entities, TScope? scope, CancellationToken cancellationToken);
}
