using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Mapster;
using Microsoft.Data.SqlClient;
using YuckQi.Data.Abstract;
using YuckQi.Data.Extensions;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Data.Sorting;
using YuckQi.Data.Sql.Dapper.Extensions;
using YuckQi.Data.Sql.Dapper.Providers.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Domain.ValueObjects;
using YuckQi.Domain.ValueObjects.Abstract;

namespace YuckQi.Data.Sql.Dapper.Providers
{
    public class SearchProvider<TEntity, TKey, TRecord> : ReadProviderBase<TRecord>, ISearchProvider<TEntity, TKey> where TEntity : IEntity<TKey> where TKey : struct
    {
        #region Constructors

        public SearchProvider(IUnitOfWork context) : base(context) { }

        #endregion


        #region Public Methods

        public async Task<IPage<TEntity>> SearchAsync(IReadOnlyCollection<IDataParameter> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (page == null)
                throw new ArgumentNullException(nameof(page));
            if (sort == null)
                throw new ArgumentNullException(nameof(sort));

            var sql = BuildSqlForSearch(parameters, page, sort);
            var records = await Context.Db.QueryAsync<TRecord>(sql, parameters.ToDynamicParameters(), Context.Transaction);
            var entities = records.Adapt<IReadOnlyCollection<TEntity>>();
            var total = await CountAsync(parameters);

            return new Page<TEntity>(entities, total, page.PageNumber, page.PageSize);
        }

        public Task<IPage<TEntity>> SearchAsync(Object parameters, IPage page, IOrderedEnumerable<SortCriteria> sort) => SearchAsync(parameters?.ToParameterCollection<SqlParameter>(), page, sort);

        #endregion


        #region Supporting Methods

        private Task<Int32> CountAsync(IReadOnlyCollection<IDataParameter> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            var sql = BuildSqlForCount(parameters);
            var total = Context.Db.ExecuteScalarAsync<Int32>(sql, parameters.ToDynamicParameters(), Context.Transaction);

            return total;
        }

        #endregion
    }
}