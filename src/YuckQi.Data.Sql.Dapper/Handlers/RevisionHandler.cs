﻿using System.Data;
using Dapper;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Handlers.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Sql.Dapper.Handlers;

public class RevisionHandler<TEntity, TIdentifier, TScope> : RevisionHandler<TEntity, TIdentifier, TScope, TEntity> where TEntity : IEntity<TIdentifier>, IRevised where TIdentifier : struct where TScope : IDbTransaction
{
    public RevisionHandler() : this(null) { }

    public RevisionHandler(RevisionOptions? options) : base(options, null) { }
}

public class RevisionHandler<TEntity, TIdentifier, TScope, TRecord> : RevisionHandlerBase<TEntity, TIdentifier, TScope> where TEntity : IEntity<TIdentifier>, IRevised where TIdentifier : struct where TScope : IDbTransaction
{
    #region Constructors

    public RevisionHandler(IMapper mapper) : base(mapper) { }

    public RevisionHandler(RevisionOptions? options, IMapper? mapper) : base(options, mapper) { }

    #endregion


    #region Protected Methods

    protected override Boolean DoRevise(TEntity entity, TScope scope) => scope.Connection.Update(MapToData<TRecord>(entity), scope) > 0;

    protected override async Task<Boolean> DoRevise(TEntity entity, TScope scope, CancellationToken cancellationToken) => await scope.Connection.UpdateAsync(MapToData<TRecord>(entity), scope, token: cancellationToken) > 0;

    #endregion
}
