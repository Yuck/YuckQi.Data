using YuckQi.Data.Extensions;
using YuckQi.Data.Filtering;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Handlers.Abstract;

public abstract class RetrievalHandlerBase<TEntity, TIdentifier, TScope> : ReadHandlerBase<TEntity>, IRetrievalHandler<TEntity, TIdentifier, TScope> where TEntity : IEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier>
{
    protected RetrievalHandlerBase(IMapper? mapper) : base(mapper) { }

    public TEntity? Get(TIdentifier identifier, TScope scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        return DoGet(identifier, scope);
    }

    public Task<TEntity?> Get(TIdentifier identifier, TScope scope, CancellationToken cancellationToken)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        return DoGet(identifier, scope, cancellationToken);
    }

    public TEntity? Get(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
    {
        if (parameters == null)
            throw new ArgumentNullException(nameof(parameters));
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        return DoGet(parameters, scope);
    }

    public Task<TEntity?> Get(IReadOnlyCollection<FilterCriteria> parameters, TScope scope, CancellationToken cancellationToken)
    {
        if (parameters == null)
            throw new ArgumentNullException(nameof(parameters));
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        return DoGet(parameters, scope, cancellationToken);
    }

    public TEntity? Get(Object parameters, TScope scope)
    {
        if (parameters == null)
            throw new ArgumentNullException(nameof(parameters));
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        return Get(parameters.ToFilterCollection(), scope);
    }

    public Task<TEntity?> Get(Object parameters, TScope scope, CancellationToken cancellationToken)
    {
        if (parameters == null)
            throw new ArgumentNullException(nameof(parameters));
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        return Get(parameters.ToFilterCollection(), scope, cancellationToken);
    }

    public IReadOnlyCollection<TEntity> GetList(TScope scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        return DoGetList(null, scope);
    }

    public Task<IReadOnlyCollection<TEntity>> GetList(TScope scope, CancellationToken cancellationToken)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        return DoGetList(null, scope, cancellationToken);
    }

    public IReadOnlyCollection<TEntity> GetList(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
    {
        if (parameters == null)
            throw new ArgumentNullException(nameof(parameters));
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        return DoGetList(parameters, scope);
    }

    public Task<IReadOnlyCollection<TEntity>> GetList(IReadOnlyCollection<FilterCriteria> parameters, TScope scope, CancellationToken cancellationToken)
    {
        if (parameters == null)
            throw new ArgumentNullException(nameof(parameters));
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        return DoGetList(parameters, scope, cancellationToken);
    }

    public IReadOnlyCollection<TEntity> GetList(Object parameters, TScope scope) => GetList(parameters.ToFilterCollection(), scope);

    public Task<IReadOnlyCollection<TEntity>> GetList(Object parameters, TScope scope, CancellationToken cancellationToken) => GetList(parameters.ToFilterCollection(), scope, cancellationToken);

    protected abstract TEntity? DoGet(TIdentifier identifier, TScope scope);

    protected abstract Task<TEntity?> DoGet(TIdentifier identifier, TScope scope, CancellationToken cancellationToken);

    protected abstract TEntity? DoGet(IReadOnlyCollection<FilterCriteria> parameters, TScope scope);

    protected abstract Task<TEntity?> DoGet(IReadOnlyCollection<FilterCriteria> parameters, TScope scope, CancellationToken cancellationToken);

    protected abstract IReadOnlyCollection<TEntity> DoGetList(IReadOnlyCollection<FilterCriteria>? parameters, TScope scope);

    protected abstract Task<IReadOnlyCollection<TEntity>> DoGetList(IReadOnlyCollection<FilterCriteria>? parameters, TScope scope, CancellationToken cancellationToken);
}
