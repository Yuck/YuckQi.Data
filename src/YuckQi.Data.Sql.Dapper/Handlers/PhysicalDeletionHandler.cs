using System.Data;
using Dapper;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Sql.Dapper.Handlers;

public class PhysicalDeletionHandler<TEntity, TIdentifier, TScope> : PhysicalDeletionHandler<TEntity, TIdentifier, TScope?, TEntity> where TEntity : IEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier> where TScope : IDbTransaction?
{
    public PhysicalDeletionHandler() : base(null) { }
}

public class PhysicalDeletionHandler<TEntity, TIdentifier, TScope, TRecord> : PhysicalDeletionHandlerBase<TEntity, TIdentifier, TScope?> where TEntity : IEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier> where TScope : IDbTransaction?
{
    public PhysicalDeletionHandler(IMapper? mapper) : base(mapper) { }

    protected override Boolean DoDelete(TEntity entity, TScope? scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        return scope.Connection.Delete(MapToData<TRecord>(entity), scope) > 0;
    }

    protected override async Task<Boolean> DoDelete(TEntity entity, TScope? scope, CancellationToken cancellationToken)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        return await scope.Connection.DeleteAsync(MapToData<TRecord>(entity), scope) > 0;
    }
}
