using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Mapster;
using YuckQi.Data.Extensions;
using YuckQi.Data.Filtering;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Data.Sorting;
using YuckQi.Data.Sql.Dapper.Extensions;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Domain.ValueObjects;
using YuckQi.Domain.ValueObjects.Abstract;

namespace YuckQi.Data.Sql.Dapper.Abstract
{
    public abstract class SearchProviderBase<TEntity, TKey, TScope, TRecord> : ISearchProvider<TEntity, TKey, TScope> where TEntity : IEntity<TKey> where TKey : struct where TScope : IDbTransaction
    {
        #region Private Members

        private readonly IReadOnlyDictionary<Type, DbType> _dbTypeMap;
        private readonly ISqlGenerator<TRecord> _sqlGenerator;

        #endregion


        #region Constructors

        public SearchProviderBase(ISqlGenerator<TRecord> sqlGenerator, IReadOnlyDictionary<Type, DbType> dbTypeMap)
        {
            _sqlGenerator = sqlGenerator ?? throw new ArgumentNullException(nameof(sqlGenerator));
            _dbTypeMap = dbTypeMap;
        }

        #endregion


        #region Public Methods

        public IPage<TEntity> Search(IReadOnlyCollection<FilterCriteria> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, TScope scope)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (page == null)
                throw new ArgumentNullException(nameof(page));
            if (sort == null)
                throw new ArgumentNullException(nameof(sort));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            var sql = _sqlGenerator.GenerateSearchQuery(parameters, page, sort);
            var records = scope.Connection.Query<TRecord>(sql, parameters.ToDynamicParameters(_dbTypeMap), scope);
            var entities = records.Adapt<IReadOnlyCollection<TEntity>>();
            var total = Count(parameters, scope);

            return new Page<TEntity>(entities, total, page.PageNumber, page.PageSize);
        }

        public async Task<IPage<TEntity>> SearchAsync(IReadOnlyCollection<FilterCriteria> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, TScope scope)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (page == null)
                throw new ArgumentNullException(nameof(page));
            if (sort == null)
                throw new ArgumentNullException(nameof(sort));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            var sql = _sqlGenerator.GenerateSearchQuery(parameters, page, sort);
            var records = await scope.Connection.QueryAsync<TRecord>(sql, parameters.ToDynamicParameters(_dbTypeMap), scope);
            var entities = records.Adapt<IReadOnlyCollection<TEntity>>();
            var total = await CountAsync(parameters, scope);

            return new Page<TEntity>(entities, total, page.PageNumber, page.PageSize);
        }

        public IPage<TEntity> Search(Object parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, TScope scope) => Search(parameters?.ToFilterCollection(), page, sort, scope);

        public Task<IPage<TEntity>> SearchAsync(Object parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, TScope scope) => SearchAsync(parameters?.ToFilterCollection(), page, sort, scope);

        #endregion


        #region Supporting Methods

        private Int32 Count(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
        {
            var sql = _sqlGenerator.GenerateCountQuery(parameters);
            var total = scope.Connection.ExecuteScalar<Int32>(sql, parameters.ToDynamicParameters(_dbTypeMap), scope);

            return total;
        }

        private Task<Int32> CountAsync(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
        {
            var sql = _sqlGenerator.GenerateCountQuery(parameters);
            var total = scope.Connection.ExecuteScalarAsync<Int32>(sql, parameters.ToDynamicParameters(_dbTypeMap), scope);

            return total;
        }

        #endregion
    }
}
