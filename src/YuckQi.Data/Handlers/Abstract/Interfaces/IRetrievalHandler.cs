using YuckQi.Data.Filtering;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Handlers.Abstract.Interfaces;

public interface IRetrievalHandler<TEntity, in TIdentifier, in TScope> where TEntity : IEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier>
{
    TEntity? Get(TIdentifier identifier, TScope? scope);

    Task<TEntity?> Get(TIdentifier identifier, TScope? scope, CancellationToken cancellationToken);

    TEntity? Get(IReadOnlyCollection<FilterCriteria> parameters, TScope? scope);

    Task<TEntity?> Get(IReadOnlyCollection<FilterCriteria> parameters, TScope? scope, CancellationToken cancellationToken);

    TEntity? Get(object parameters, TScope? scope);

    Task<TEntity?> Get(object parameters, TScope? scope, CancellationToken cancellationToken);

    IReadOnlyCollection<TEntity> GetList(TScope? scope);

    Task<IReadOnlyCollection<TEntity>> GetList(TScope? scope, CancellationToken cancellationToken);

    IReadOnlyCollection<TEntity> GetList(IReadOnlyCollection<FilterCriteria> parameters, TScope? scope);

    Task<IReadOnlyCollection<TEntity>> GetList(IReadOnlyCollection<FilterCriteria> parameters, TScope? scope, CancellationToken cancellationToken);

    IReadOnlyCollection<TEntity> GetList(object parameters, TScope? scope);

    Task<IReadOnlyCollection<TEntity>> GetList(object parameters, TScope? scope, CancellationToken cancellationToken);
}
