using YuckQi.Data.Exceptions;
using YuckQi.Data.Handlers.Write.Abstract.Interfaces;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Handlers.Write.Abstract;

public abstract class PhysicalDeletionHandlerBase<TEntity, TIdentifier, TScope> : PhysicalDeletionHandlerBase<TEntity, TIdentifier, TScope, TEntity> where TEntity : IEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier>
{
    protected PhysicalDeletionHandlerBase() : this(null) { }

    protected PhysicalDeletionHandlerBase(IMapper? mapper) : base(mapper) { }
}

public abstract class PhysicalDeletionHandlerBase<TEntity, TIdentifier, TScope, TData> : HandlerBase<TEntity, TData>, IPhysicalDeletionHandler<TEntity, TIdentifier, TScope?> where TEntity : IEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier>
{
    protected PhysicalDeletionHandlerBase() : this(null) { }

    protected PhysicalDeletionHandlerBase(IMapper? mapper) : base(mapper) { }

    public TEntity Delete(TEntity entity, TScope? scope)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        if (! DoDelete(entity, scope))
            throw new PhysicalDeletionException<TEntity, TIdentifier>(entity.Identifier);

        return entity;
    }

    public async Task<TEntity> Delete(TEntity entity, TScope? scope, CancellationToken cancellationToken)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        if (! await DoDelete(entity, scope, cancellationToken))
            throw new PhysicalDeletionException<TEntity, TIdentifier>(entity.Identifier);

        return entity;
    }

    protected abstract Boolean DoDelete(TEntity entity, TScope? scope);

    protected abstract Task<Boolean> DoDelete(TEntity entity, TScope? scope, CancellationToken cancellationToken);
}
