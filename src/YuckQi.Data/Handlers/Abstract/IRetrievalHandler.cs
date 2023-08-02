using YuckQi.Data.Filtering;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Handlers.Abstract;

public interface IRetrievalHandler<TEntity, in TIdentifier, in TScope> where TEntity : IEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier>
{
    TEntity? Get(TIdentifier identifier, TScope? scope);

    Task<TEntity?> Get(TIdentifier identifier, TScope? scope, CancellationToken cancellationToken);

    TEntity? Get(IReadOnlyCollection<FilterCriteria> parameters, TScope? scope);

    Task<TEntity?> Get(IReadOnlyCollection<FilterCriteria> parameters, TScope? scope, CancellationToken cancellationToken);

    TEntity? Get(Object parameters, TScope? scope);

    Task<TEntity?> Get(Object parameters, TScope? scope, CancellationToken cancellationToken);

    IReadOnlyCollection<TEntity> GetList(TScope? scope);

    Task<IReadOnlyCollection<TEntity>> GetList(TScope? scope, CancellationToken cancellationToken);

    IReadOnlyCollection<TEntity> GetList(IReadOnlyCollection<FilterCriteria> parameters, TScope? scope);

    Task<IReadOnlyCollection<TEntity>> GetList(IReadOnlyCollection<FilterCriteria> parameters, TScope? scope, CancellationToken cancellationToken);

    IReadOnlyCollection<TEntity> GetList(Object parameters, TScope? scope);

    Task<IReadOnlyCollection<TEntity>> GetList(Object parameters, TScope? scope, CancellationToken cancellationToken);
}
