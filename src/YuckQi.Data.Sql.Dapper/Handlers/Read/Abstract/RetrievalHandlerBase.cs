using System.Data;
using Dapper;
using YuckQi.Data.Filtering;
using YuckQi.Data.Sql.Dapper.Abstract.Interfaces;
using YuckQi.Data.Sql.Dapper.Extensions;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Sql.Dapper.Handlers.Read.Abstract;

public class RetrievalHandlerBase<TEntity, TIdentifier, TScope> : RetrievalHandlerBase<TEntity, TIdentifier, TScope?, TEntity> where TEntity : IEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier> where TScope : IDbTransaction?
{
    protected RetrievalHandlerBase(ISqlGenerator sqlGenerator, IReadOnlyDictionary<Type, DbType> dbTypeMap) : base(sqlGenerator, dbTypeMap, null) { }
}

public class RetrievalHandlerBase<TEntity, TIdentifier, TScope, TRecord> : Data.Handlers.Read.Abstract.RetrievalHandlerBase<TEntity, TIdentifier, TScope?, TRecord> where TEntity : IEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier> where TScope : IDbTransaction?
{
    private readonly IReadOnlyDictionary<Type, DbType> _dbTypeMap;
    private readonly ISqlGenerator _sqlGenerator;

    protected RetrievalHandlerBase(ISqlGenerator sqlGenerator, IReadOnlyDictionary<Type, DbType> dbTypeMap, IMapper? mapper) : base(mapper)
    {
        _sqlGenerator = sqlGenerator ?? throw new ArgumentNullException(nameof(sqlGenerator));
        _dbTypeMap = dbTypeMap;
    }

    protected override TEntity? DoGet(TIdentifier identifier, TScope? scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var record = scope.Connection.Get<TRecord>(identifier, scope);
        var entity = MapToEntity(record);

        return entity;
    }

    protected override async Task<TEntity?> DoGet(TIdentifier identifier, TScope? scope, CancellationToken cancellationToken)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var record = await scope.Connection.GetAsync<TRecord>(identifier, scope);
        var entity = MapToEntity(record);

        return entity;
    }

    protected override TEntity? DoGet(IReadOnlyCollection<FilterCriteria> parameters, TScope? scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var sql = _sqlGenerator.GenerateGetQuery(parameters);
        var record = scope.Connection.QuerySingleOrDefault<TRecord>(sql, parameters.ToDynamicParameters(_dbTypeMap), scope);
        var entity = MapToEntity(record);

        return entity;
    }

    protected override async Task<TEntity?> DoGet(IReadOnlyCollection<FilterCriteria> parameters, TScope? scope, CancellationToken cancellationToken)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var sql = _sqlGenerator.GenerateGetQuery(parameters);
        var record = await scope.Connection.QuerySingleOrDefaultAsync<TRecord>(sql, parameters.ToDynamicParameters(_dbTypeMap), scope);
        var entity = MapToEntity(record);

        return entity;
    }

    protected override IReadOnlyCollection<TEntity> DoGetList(IReadOnlyCollection<FilterCriteria>? parameters, TScope? scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var sql = _sqlGenerator.GenerateGetQuery(parameters);
        var records = scope.Connection.Query<TRecord>(sql, parameters?.ToDynamicParameters(_dbTypeMap), scope);
        var entities = MapToEntityCollection(records);

        return entities;
    }

    protected override async Task<IReadOnlyCollection<TEntity>> DoGetList(IReadOnlyCollection<FilterCriteria>? parameters, TScope? scope, CancellationToken cancellationToken)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var sql = _sqlGenerator.GenerateGetQuery(parameters);
        var records = await scope.Connection.QueryAsync<TRecord>(sql, parameters?.ToDynamicParameters(_dbTypeMap), scope);
        var entities = MapToEntityCollection(records);

        return entities;
    }
}
