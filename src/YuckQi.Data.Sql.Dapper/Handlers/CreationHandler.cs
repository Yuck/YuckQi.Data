using System.Data;
using CSharpFunctionalExtensions;
using Dapper;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Handlers.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Sql.Dapper.Handlers;

public class CreationHandler<TEntity, TIdentifier, TScope> : CreationHandler<TEntity, TIdentifier, TScope?, TEntity> where TEntity : IEntity<TIdentifier>, ICreated where TIdentifier : IEquatable<TIdentifier> where TScope : IDbTransaction?
{
    public CreationHandler() : this(null) { }

    public CreationHandler(CreationOptions<TIdentifier>? options) : base(options, null) { }
}

public class CreationHandler<TEntity, TIdentifier, TScope, TRecord> : CreationHandlerBase<TEntity, TIdentifier, TScope?> where TEntity : IEntity<TIdentifier>, ICreated where TIdentifier : IEquatable<TIdentifier> where TScope : IDbTransaction?
{
    public CreationHandler(IMapper? mapper) : base(mapper) { }

    public CreationHandler(CreationOptions<TIdentifier>? options, IMapper? mapper) : base(options, mapper) { }

    protected override Maybe<TIdentifier?> DoCreate(TEntity entity, TScope? scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var record = MapToData<TRecord>(entity) ?? throw new InvalidOperationException();

        return Maybe.From(scope.Connection.Insert<TIdentifier?, TRecord>(record, scope));
    }

    protected override async Task<Maybe<TIdentifier?>> DoCreate(TEntity entity, TScope? scope, CancellationToken cancellationToken)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var record = MapToData<TRecord>(entity) ?? throw new InvalidOperationException();

        return Maybe.From(await scope.Connection.InsertAsync<TIdentifier?, TRecord>(record, scope));
    }
}
