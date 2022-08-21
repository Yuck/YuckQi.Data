using System.Data;
using Dapper;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Handlers.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Sql.Dapper.Handlers;

public class CreationHandler<TEntity, TIdentifier, TScope> : CreationHandler<TEntity, TIdentifier, TScope, TEntity> where TEntity : IEntity<TIdentifier>, ICreated where TIdentifier : struct where TScope : IDbTransaction
{
    public CreationHandler() : this(null) { }

    public CreationHandler(CreationOptions<TIdentifier>? options) : base(options, null) { }
}

public class CreationHandler<TEntity, TIdentifier, TScope, TRecord> : CreationHandlerBase<TEntity, TIdentifier, TScope> where TEntity : IEntity<TIdentifier>, ICreated where TIdentifier : struct where TScope : IDbTransaction
{
    #region Constructors

    public CreationHandler(IMapper? mapper) : base(mapper) { }

    public CreationHandler(CreationOptions<TIdentifier>? options, IMapper? mapper) : base(options, mapper) { }

    #endregion


    #region Protected Methods

    protected override TIdentifier? DoCreate(TEntity entity, TScope scope)
    {
        var record = MapToData<TRecord>(entity);
        if (record == null)
            throw new NullReferenceException();

        return scope.Connection.Insert<TIdentifier?, TRecord>(record, scope);
    }

    protected override Task<TIdentifier?> DoCreate(TEntity entity, TScope scope, CancellationToken cancellationToken)
    {
        var record = MapToData<TRecord>(entity);
        if (record == null)
            throw new NullReferenceException();

        return scope.Connection.InsertAsync<TIdentifier?, TRecord>(record, scope);
    }

    #endregion
}
