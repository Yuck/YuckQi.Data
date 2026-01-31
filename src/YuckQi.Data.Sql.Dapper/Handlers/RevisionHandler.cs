using System.Data;
using Dapper;
using YuckQi.Data.Handlers.Write.Abstract;
using YuckQi.Data.Handlers.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Sql.Dapper.Handlers;

public class RevisionHandler<TEntity, TIdentifier, TScope> : RevisionHandler<TEntity, TIdentifier, TScope?, TEntity> where TEntity : IEntity<TIdentifier>, IRevised where TIdentifier : IEquatable<TIdentifier> where TScope : IDbTransaction?
{
    public RevisionHandler() : this(null) { }

    public RevisionHandler(RevisionOptions? options) : base(options, null) { }
}

public class RevisionHandler<TEntity, TIdentifier, TScope, TRecord> : RevisionHandlerBase<TEntity, TIdentifier, TScope?, TRecord> where TEntity : IEntity<TIdentifier>, IRevised where TIdentifier : IEquatable<TIdentifier> where TScope : IDbTransaction?
{
    public RevisionHandler(IMapper mapper) : base(mapper) { }

    public RevisionHandler(RevisionOptions? options, IMapper? mapper) : base(options, mapper) { }

    protected override Boolean DoRevise(TEntity entity, TScope? scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        return scope.Connection.Update(MapToData(entity), scope) > 0;
    }

    protected override async Task<Boolean> DoRevise(TEntity entity, TScope? scope, CancellationToken cancellationToken)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        return await scope.Connection.UpdateAsync(MapToData(entity), scope, token: cancellationToken) > 0;
    }
}
