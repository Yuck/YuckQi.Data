using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Mapster;
using YuckQi.Data.Abstract;
using YuckQi.Data.Extensions;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Sorting.Abstract;
using YuckQi.Data.Sql.Dapper.Handlers.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Domain.ValueObjects.Abstract;

namespace YuckQi.Data.Sql.Dapper.Handlers
{
    public class SearchHandler<TEntity, TKey, TRecord> : ReadHandlerBase<TRecord>, ISearchHandler<TEntity, TKey> where TEntity : IEntity<TKey> where TKey : struct
    {
        #region Constructors

        public SearchHandler(IUnitOfWork uow) : base(uow)
        {
        }

        #endregion


        #region Public Methods

        public async Task<IReadOnlyCollection<TEntity>> SearchAsync(IReadOnlyCollection<IDataParameter> parameters, IPage page, IOrderedEnumerable<ISortExpression> sort)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (page == null)
                throw new ArgumentNullException(nameof(page));
            if (sort == null)
                throw new ArgumentNullException(nameof(sort));

            var sql = BuildParameterizedSql(parameters, page, sort);
            var records = await Db.QueryAsync<TRecord>(sql, parameters, Transaction);
            var entities = records.Adapt<IReadOnlyCollection<TEntity>>();

            return entities;
        }

        public Task<IReadOnlyCollection<TEntity>> SearchAsync(object parameters, IPage page, IOrderedEnumerable<ISortExpression> sort)
        {
            return SearchAsync(parameters?.ToParameterCollection<SqlParameter>(), page, sort);
        }

        #endregion
    }
}