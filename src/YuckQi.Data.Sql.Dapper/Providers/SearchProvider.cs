using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Mapster;
using YuckQi.Data.Abstract;
using YuckQi.Data.Extensions;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Data.Sorting;
using YuckQi.Data.Sql.Dapper.Abstract;
using YuckQi.Data.Sql.Dapper.Extensions;
using YuckQi.Data.Sql.Dapper.Providers.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Domain.ValueObjects;
using YuckQi.Domain.ValueObjects.Abstract;

namespace YuckQi.Data.Sql.Dapper.Providers
{
    public class SearchProvider<TEntity, TKey, TRecord, TDataParameter> : DataProviderBase, ISearchProvider<TEntity, TKey, TDataParameter> where TEntity : IEntity<TKey> where TKey : struct where TDataParameter : IDataParameter, new()
    {
        #region Private Members

        private readonly ISqlGenerator<TDataParameter> _sqlGenerator;

        #endregion


        #region Constructors

        public SearchProvider(IUnitOfWork context, ISqlGenerator<TDataParameter> sqlGenerator) : base(context)
        {
            _sqlGenerator = sqlGenerator ?? throw new ArgumentNullException(nameof(sqlGenerator));
        }

        #endregion


        #region Public Methods

        public async Task<IPage<TEntity>> SearchAsync(IReadOnlyCollection<TDataParameter> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (page == null)
                throw new ArgumentNullException(nameof(page));
            if (sort == null)
                throw new ArgumentNullException(nameof(sort));

            var sql = _sqlGenerator.GenerateSearchQuery(parameters, page, sort);
            var records = await Context.Db.QueryAsync<TRecord>(sql, parameters.ToDynamicParameters(), Context.Transaction);
            var entities = records.Adapt<IReadOnlyCollection<TEntity>>();
            var total = await CountAsync(parameters);

            return new Page<TEntity>(entities, total, page.PageNumber, page.PageSize);
        }

        public Task<IPage<TEntity>> SearchAsync(Object parameters, IPage page, IOrderedEnumerable<SortCriteria> sort) => SearchAsync(parameters?.ToParameterCollection<TDataParameter>(), page, sort);

        #endregion


        #region Supporting Methods

        private Task<Int32> CountAsync(IReadOnlyCollection<TDataParameter> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            var sql = _sqlGenerator.GenerateCountQuery(parameters);
            var total = Context.Db.ExecuteScalarAsync<Int32>(sql, parameters.ToDynamicParameters(), Context.Transaction);

            return total;
        }

        #endregion
    }
}