using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using YuckQi.Data.Abstract;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Data.Sorting;
using YuckQi.Data.Sql.Dapper.MySql.Providers.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Domain.ValueObjects.Abstract;

namespace YuckQi.Data.Sql.Dapper.MySql.Providers
{
    public class SearchProvider<TEntity, TKey, TRecord> : ReadProviderBase<TRecord>, ISearchProvider<TEntity, TKey> where TEntity : IEntity<TKey> where TKey : struct
    {
        #region Constructors

        public SearchProvider(IUnitOfWork context) : base(context) { }

        #endregion


        #region Public Methods

        public Task<IPage<TEntity>> SearchAsync(IReadOnlyCollection<IDataParameter> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort) => throw new NotImplementedException();

        public Task<IPage<TEntity>> SearchAsync(Object parameters, IPage page, IOrderedEnumerable<SortCriteria> sort) => throw new NotImplementedException();

        #endregion
    }
}