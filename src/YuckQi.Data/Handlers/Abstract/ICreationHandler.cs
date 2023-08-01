using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Handlers.Abstract;

public interface ICreationHandler<TEntity, TIdentifier, in TScope> where TEntity : IEntity<TIdentifier>, ICreated where TIdentifier : IEquatable<TIdentifier>
{
    TEntity Create(TEntity entity, TScope scope);

    IEnumerable<TEntity> Create(IEnumerable<TEntity> entities, TScope scope);

    Task<TEntity> Create(TEntity entity, TScope scope, CancellationToken cancellationToken);

    Task<IEnumerable<TEntity>> Create(IEnumerable<TEntity> entities, TScope scope, CancellationToken cancellationToken);
}
