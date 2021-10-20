using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Mapster;
using YuckQi.Data.Filtering;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Sorting;
using YuckQi.Data.Sql.Dapper.Extensions;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Domain.ValueObjects.Abstract;

namespace YuckQi.Data.Sql.Dapper.Abstract
{
    public abstract class SearchProviderBase<TEntity, TKey, TScope, TRecord> : SearchHandlerBase<TEntity, TKey, TScope> where TEntity : IEntity<TKey> where TKey : struct where TScope : IDbTransaction
    {
        #region Private Members

        private readonly IReadOnlyDictionary<Type, DbType> _dbTypeMap;
        private readonly ISqlGenerator<TRecord> _sqlGenerator;

        #endregion


        #region Constructors

        protected SearchProviderBase(ISqlGenerator<TRecord> sqlGenerator, IReadOnlyDictionary<Type, DbType> dbTypeMap)
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

        protected override Task<Int32> DoCountAsync(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
        {
            var sql = _sqlGenerator.GenerateCountQuery(parameters);
            var total = scope.Connection.ExecuteScalarAsync<Int32>(sql, parameters.ToDynamicParameters(_dbTypeMap), scope);

            return total;
        }

        protected override IReadOnlyCollection<TEntity> DoSearch(IReadOnlyCollection<FilterCriteria> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, TScope scope)
        {
            var sql = _sqlGenerator.GenerateSearchQuery(parameters, page, sort);
            var records = scope.Connection.Query<TRecord>(sql, parameters.ToDynamicParameters(_dbTypeMap), scope);
            var entities = records.Adapt<IReadOnlyCollection<TEntity>>();

            return entities;
        }

        protected override async Task<IReadOnlyCollection<TEntity>> DoSearchAsync(IReadOnlyCollection<FilterCriteria> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, TScope scope)
        {
            var sql = _sqlGenerator.GenerateSearchQuery(parameters, page, sort);
            var records = await scope.Connection.QueryAsync<TRecord>(sql, parameters.ToDynamicParameters(_dbTypeMap), scope);
            var entities = records.Adapt<IReadOnlyCollection<TEntity>>();

            return entities;
        }

        #endregion
    }
}
