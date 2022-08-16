﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using YuckQi.Data.Filtering;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Sql.Dapper.Abstract;
using YuckQi.Data.Sql.Dapper.Extensions;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Sql.Dapper.Handlers.Abstract;

public class RetrievalHandlerBase<TEntity, TIdentifier, TScope, TRecord> : RetrievalHandlerBase<TEntity, TIdentifier, TScope> where TEntity : IEntity<TIdentifier> where TIdentifier : struct where TScope : IDbTransaction
{
    #region Private Members

    private readonly IReadOnlyDictionary<Type, DbType> _dbTypeMap;
    private readonly ISqlGenerator<TRecord> _sqlGenerator;

    #endregion


    #region Constructors

    protected RetrievalHandlerBase(ISqlGenerator<TRecord> sqlGenerator, IReadOnlyDictionary<Type, DbType> dbTypeMap, IMapper mapper) : base(mapper)
    {
        _sqlGenerator = sqlGenerator ?? throw new ArgumentNullException(nameof(sqlGenerator));
        _dbTypeMap = dbTypeMap;
    }

    #endregion


    #region Public Methods

    protected override TEntity DoGet(TIdentifier identifier, TScope scope)
    {
        var record = scope.Connection.Get<TRecord>(identifier, scope);
        var entity = Mapper.Map<TEntity>(record);

        return entity;
    }

    protected override async Task<TEntity> DoGet(TIdentifier identifier, TScope scope, CancellationToken cancellationToken)
    {
        var record = await scope.Connection.GetAsync<TRecord>(identifier, scope);
        var entity = Mapper.Map<TEntity>(record);

        return entity;
    }

    protected override TEntity DoGet(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
    {
        var sql = _sqlGenerator.GenerateGetQuery(parameters);
        var record = scope.Connection.QuerySingleOrDefault<TRecord>(sql, parameters.ToDynamicParameters(_dbTypeMap), scope);
        var entity = Mapper.Map<TEntity>(record);

        return entity;
    }

    protected override async Task<TEntity> DoGet(IReadOnlyCollection<FilterCriteria> parameters, TScope scope, CancellationToken cancellationToken)
    {
        var sql = _sqlGenerator.GenerateGetQuery(parameters);
        var record = await scope.Connection.QuerySingleOrDefaultAsync<TRecord>(sql, parameters.ToDynamicParameters(_dbTypeMap), scope);
        var entity = Mapper.Map<TEntity>(record);

        return entity;
    }

    protected override IReadOnlyCollection<TEntity> DoGetList(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
    {
        var sql = _sqlGenerator.GenerateGetQuery(parameters);
        var records = scope.Connection.Query<TRecord>(sql, parameters?.ToDynamicParameters(_dbTypeMap), scope);
        var entities = Mapper.Map<IReadOnlyCollection<TEntity>>(records);

        return entities;
    }

    protected override async Task<IReadOnlyCollection<TEntity>> DoGetList(IReadOnlyCollection<FilterCriteria> parameters, TScope scope, CancellationToken cancellationToken)
    {
        var sql = _sqlGenerator.GenerateGetQuery(parameters);
        var records = await scope.Connection.QueryAsync<TRecord>(sql, parameters?.ToDynamicParameters(_dbTypeMap), scope);
        var entities = Mapper.Map<IReadOnlyCollection<TEntity>>(records);

        return entities;
    }

    #endregion
}
