using System.Data;
using Dapper;
using YuckQi.Data.Filtering;
using YuckQi.Data.Sorting;
using YuckQi.Data.Sql.Dapper.Abstract;
using YuckQi.Data.Sql.Dapper.Extensions;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Domain.ValueObjects.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Sql.Dapper.Handlers.Abstract;

public abstract class SearchHandlerBase<TEntity, TIdentifier, TScope> : SearchHandlerBase<TEntity, TIdentifier, TScope, TEntity> where TEntity : IEntity<TIdentifier> where TIdentifier : struct where TScope : IDbTransaction
{
    protected SearchHandlerBase(ISqlGenerator<TEntity> sqlGenerator, IReadOnlyDictionary<Type, DbType> dbTypeMap) : base(sqlGenerator, dbTypeMap, null) { }
}

public abstract class SearchHandlerBase<TEntity, TIdentifier, TScope, TRecord> : Data.Handlers.Abstract.SearchHandlerBase<TEntity, TIdentifier, TScope> where TEntity : IEntity<TIdentifier> where TIdentifier : struct where TScope : IDbTransaction
{
    #region Private Members

    private readonly IReadOnlyDictionary<Type, DbType> _dbTypeMap;
    private readonly ISqlGenerator<TRecord> _sqlGenerator;

    #endregion


    #region Constructors

    protected SearchHandlerBase(ISqlGenerator<TRecord> sqlGenerator, IReadOnlyDictionary<Type, DbType> dbTypeMap, IMapper? mapper) : base(mapper)
    {
        _sqlGenerator = sqlGenerator ?? throw new ArgumentNullException(nameof(sqlGenerator));
        _dbTypeMap = dbTypeMap;
    }

    #endregion


    #region Protected Methods

    protected override Int32 DoCount(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
    {
        var sql = _sqlGenerator.GenerateCountQuery(parameters);
        var total = scope.Connection.ExecuteScalar<Int32>(sql, parameters.ToDynamicParameters(_dbTypeMap), scope);

        return total;
    }

    protected override Task<Int32> DoCount(IReadOnlyCollection<FilterCriteria> parameters, TScope scope, CancellationToken cancellationToken)
    {
        var sql = _sqlGenerator.GenerateCountQuery(parameters);
        var total = scope.Connection.ExecuteScalarAsync<Int32>(sql, parameters.ToDynamicParameters(_dbTypeMap), scope);

        return total;
    }

    protected override IReadOnlyCollection<TEntity> DoSearch(IReadOnlyCollection<FilterCriteria> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, TScope scope)
    {
        var sql = _sqlGenerator.GenerateSearchQuery(parameters, page, sort);
        var records = scope.Connection.Query<TRecord>(sql, parameters.ToDynamicParameters(_dbTypeMap), scope);
        var entities = MapToEntityCollection(records);

        return entities;
    }

    protected override async Task<IReadOnlyCollection<TEntity>> DoSearch(IReadOnlyCollection<FilterCriteria> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, TScope scope, CancellationToken cancellationToken)
    {
        var sql = _sqlGenerator.GenerateSearchQuery(parameters, page, sort);
        var records = await scope.Connection.QueryAsync<TRecord>(sql, parameters.ToDynamicParameters(_dbTypeMap), scope);
        var entities = MapToEntityCollection(records);

        return entities;
    }

    #endregion
}
