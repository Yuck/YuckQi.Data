using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Handlers.Abstract;

public interface ICreationHandler<TEntity, TIdentifier, in TScope> where TEntity : IEntity<TIdentifier>, ICreated where TIdentifier : struct
{
    TEntity Create(TEntity entity, TScope scope);

    Task<TEntity> Create(TEntity entity, TScope scope, CancellationToken cancellationToken);
}
