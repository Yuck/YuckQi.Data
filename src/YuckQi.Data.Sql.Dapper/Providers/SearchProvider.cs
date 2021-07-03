using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Mapster;
using YuckQi.Data.Extensions;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Data.Sorting;
using YuckQi.Data.Sql.Dapper.Abstract;
using YuckQi.Data.Sql.Dapper.Extensions;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Domain.ValueObjects;
using YuckQi.Domain.ValueObjects.Abstract;

namespace YuckQi.Data.Sql.Dapper.Providers
{
    public class SearchProvider<TEntity, TKey, TRecord, TDataParameter> : ISearchProvider<TEntity, TKey, TDataParameter> where TEntity : IEntity<TKey> where TKey : struct where TDataParameter : IDataParameter, new()
    {
        #region Private Members

        private readonly ISqlGenerator<TRecord, TDataParameter> _sqlGenerator;

        #endregion


        #region Constructors

        public SearchProvider(ISqlGenerator<TRecord, TDataParameter> sqlGenerator)
        {
            _sqlGenerator = sqlGenerator ?? throw new ArgumentNullException(nameof(sqlGenerator));
        }

        #endregion


        #region Public Methods

        public IPage<TEntity> Search(IReadOnlyCollection<TDataParameter> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, IDbTransaction transaction)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (page == null)
                throw new ArgumentNullException(nameof(page));
            if (sort == null)
                throw new ArgumentNullException(nameof(sort));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            var sql = _sqlGenerator.GenerateSearchQuery(parameters, page, sort);
            var records = transaction.Connection.Query<TRecord>(sql, parameters.ToDynamicParameters(), transaction);
            var entities = records.Adapt<IReadOnlyCollection<TEntity>>();
            var total = Count(parameters, transaction);

            return new Page<TEntity>(entities, total, page.PageNumber, page.PageSize);
        }

        public async Task<IPage<TEntity>> SearchAsync(IReadOnlyCollection<TDataParameter> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, IDbTransaction transaction)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (page == null)
                throw new ArgumentNullException(nameof(page));
            if (sort == null)
                throw new ArgumentNullException(nameof(sort));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            var sql = _sqlGenerator.GenerateSearchQuery(parameters, page, sort);
            var records = await transaction.Connection.QueryAsync<TRecord>(sql, parameters.ToDynamicParameters(), transaction);
            var entities = records.Adapt<IReadOnlyCollection<TEntity>>();
            var total = await CountAsync(parameters, transaction);

            return new Page<TEntity>(entities, total, page.PageNumber, page.PageSize);
        }

        public IPage<TEntity> Search(Object parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, IDbTransaction transaction) => Search(parameters?.ToParameterCollection<TDataParameter>(), page, sort, transaction);
        public Task<IPage<TEntity>> SearchAsync(Object parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, IDbTransaction transaction) => SearchAsync(parameters?.ToParameterCollection<TDataParameter>(), page, sort, transaction);

        #endregion


        #region Supporting Methods

        private Int32 Count(IReadOnlyCollection<TDataParameter> parameters, IDbTransaction transaction)
        {
            var sql = _sqlGenerator.GenerateCountQuery(parameters);
            var total = transaction.Connection.ExecuteScalar<Int32>(sql, parameters.ToDynamicParameters(), transaction);

            return total;
        }

        private Task<Int32> CountAsync(IReadOnlyCollection<TDataParameter> parameters, IDbTransaction transaction)
        {
            var sql = _sqlGenerator.GenerateCountQuery(parameters);
            var total = transaction.Connection.ExecuteScalarAsync<Int32>(sql, parameters.ToDynamicParameters(), transaction);

            return total;
        }

        #endregion
    }
}