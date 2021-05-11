using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using YuckQi.Data.Abstract;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Data.Sql.Dapper.MySql.Providers.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.MySql.Providers
{
    public class RetrievalProvider<TEntity, TKey, TRecord> : ReadProviderBase<TRecord>, IRetrievalProvider<TEntity, TKey> where TEntity : IEntity<TKey> where TKey : struct
    {
        #region Constructors

        public RetrievalProvider(IUnitOfWork context) : base(context) { }

        #endregion


        #region Public Methods

        public Task<TEntity> GetAsync(TKey key) => throw new NotImplementedException();

        public Task<TEntity> GetAsync(IReadOnlyCollection<IDataParameter> parameters) => throw new NotImplementedException();

        public Task<TEntity> GetAsync(Object parameters) => throw new NotImplementedException();

        public Task<IReadOnlyCollection<TEntity>> GetListAsync(IReadOnlyCollection<IDataParameter> parameters = null) => throw new NotImplementedException();

        public Task<IReadOnlyCollection<TEntity>> GetListAsync(Object parameters = null) => throw new NotImplementedException();

        #endregion
    }
}