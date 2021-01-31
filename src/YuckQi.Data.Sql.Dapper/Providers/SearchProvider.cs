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
using YuckQi.Data.Sorting.Abstract;
using YuckQi.Data.Sql.Dapper.Providers.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Domain.ValueObjects;
using YuckQi.Domain.ValueObjects.Abstract;

namespace YuckQi.Data.Sql.Dapper.Providers
{
    public class SearchProvider<TEntity, TKey, TRecord> : ReadProviderBase<TRecord>, ISearchProvider<TEntity, TKey> where TEntity : IEntity<TKey> where TKey : struct
    {
        #region Constructors

        public SearchProvider(IUnitOfWork uow) : base(uow)
        {
        }

        #endregion


        #region Public Methods

        public async Task<IPage<TEntity>> SearchAsync<TSortExpression>(IReadOnlyCollection<IDataParameter> parameters, IPage page, IOrderedEnumerable<ISortExpression<TSortExpression>> sort) where TSortExpression : class
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (page == null)
                throw new ArgumentNullException(nameof(page));
            if (sort == null)
                throw new ArgumentNullException(nameof(sort));

            var sql = BuildSqlForSearch(parameters, page, sort);
            var records = await Db.QueryAsync<TRecord>(sql, parameters, Transaction);
            var entities = records.Adapt<IReadOnlyCollection<TEntity>>();
            var total = await CountAsync(parameters);

            return new Page<TEntity>(entities, total, page.PageNumber, page.PageSize);
        }

        public Task<IPage<TEntity>> SearchAsync<TSortExpression>(object parameters, IPage page, IOrderedEnumerable<ISortExpression<TSortExpression>> sort) where TSortExpression : class
        {
            return SearchAsync(parameters?.ToParameterCollection<SqlParameter>(), page, sort);
        }

        #endregion


        #region Supporting Methods

        private Task<int> CountAsync(IReadOnlyCollection<IDataParameter> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            var sql = BuildSqlForCount(parameters);
            var total = Db.RecordCountAsync<TRecord>(sql, parameters, Transaction);

            return total;
        }

        #endregion
    }
}