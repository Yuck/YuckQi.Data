using System.Collections.Concurrent;
using YuckQi.Data.Extensions;
using YuckQi.Data.Filtering;
using YuckQi.Data.Handlers.Read.Abstract.Interfaces;
using YuckQi.Data.MemDb.Filtering;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.MemDb.Handlers;

public class RetrievalHandler<TEntity, TIdentifier, TScope> : IRetrievalHandler<TEntity, TIdentifier, TScope?> where TEntity : IEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier>
{
    private readonly ConcurrentDictionary<TIdentifier, TEntity> _entities;

    public RetrievalHandler(ConcurrentDictionary<TIdentifier, TEntity> entities)
    {
        _entities = entities ?? throw new ArgumentNullException(nameof(entities));
    }

    public TEntity? Get(TIdentifier identifier, TScope? scope) => _entities.TryGetValue(identifier, out var entity) ? entity : default;

    public Task<TEntity?> Get(TIdentifier identifier, TScope? scope, CancellationToken cancellationToken) => Task.FromResult(Get(identifier, scope));

    public TEntity? Get(IReadOnlyCollection<FilterCriteria> parameters, TScope? scope)
    {
        var entities = GetEntities(parameters);
        var entity = entities.SingleOrDefault();

        return entity;
    }

    public Task<TEntity?> Get(IReadOnlyCollection<FilterCriteria> parameters, TScope? scope, CancellationToken cancellationToken) => Task.FromResult(Get(parameters, scope));

    public TEntity? Get(Object parameters, TScope? scope) => Get(parameters.ToFilterCollection(), scope);

    public Task<TEntity?> Get(Object parameters, TScope? scope, CancellationToken cancellationToken) => Task.FromResult(Get(parameters, scope));

    public IReadOnlyCollection<TEntity> GetList(TScope? scope) => _entities.Values.Select(t => t).ToList();

    public Task<IReadOnlyCollection<TEntity>> GetList(TScope? scope, CancellationToken cancellationToken) => Task.FromResult(GetList(scope));

    public IReadOnlyCollection<TEntity> GetList(IReadOnlyCollection<FilterCriteria> parameters, TScope? scope) => GetEntities(parameters).ToList();

    public Task<IReadOnlyCollection<TEntity>> GetList(IReadOnlyCollection<FilterCriteria> parameters, TScope? scope, CancellationToken cancellationToken) => Task.FromResult(GetList(parameters, scope));

    public IReadOnlyCollection<TEntity> GetList(Object parameters, TScope? scope) => GetList(parameters.ToFilterCollection(), scope);

    public Task<IReadOnlyCollection<TEntity>> GetList(Object parameters, TScope? scope, CancellationToken cancellationToken) => Task.FromResult(GetList(parameters, scope));

    private IEnumerable<TEntity> GetEntities(IReadOnlyCollection<FilterCriteria> parameters)
    {
        return _entities.Values.Where(entity => parameters.Select(t => t.ToExpression(entity)).All(t => t()));
    }
}
