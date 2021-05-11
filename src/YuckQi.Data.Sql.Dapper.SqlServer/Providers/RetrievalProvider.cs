using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Mapster;
using Microsoft.Data.SqlClient;
using YuckQi.Data.Abstract;
using YuckQi.Data.Extensions;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Data.Sql.Dapper.SqlServer.Extensions;
using YuckQi.Data.Sql.Dapper.SqlServer.Providers.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.SqlServer.Providers
{
    public class RetrievalProvider<TEntity, TKey, TRecord> : ReadProviderBase<TRecord>, IRetrievalProvider<TEntity, TKey> where TEntity : IEntity<TKey> where TKey : struct
    {
        #region Constructors

        public RetrievalProvider(IUnitOfWork context) : base(context) { }

        #endregion


        #region Public Methods

        public async Task<TEntity> GetAsync(TKey key)
        {
            var record = await Context.Db.GetAsync<TRecord>(key, Context.Transaction);
            var entity = record.Adapt<TEntity>();

            return entity;
        }

        public async Task<TEntity> GetAsync(IReadOnlyCollection<IDataParameter> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            var sql = BuildSqlForGet(parameters);
            var record = await Context.Db.QuerySingleOrDefaultAsync<TRecord>(sql, parameters.ToDynamicParameters(), Context.Transaction);
            var entity = record.Adapt<TEntity>();

            return entity;
        }

        public Task<TEntity> GetAsync(Object parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            return GetAsync(parameters.ToParameterCollection<SqlParameter>());
        }

        public async Task<IReadOnlyCollection<TEntity>> GetListAsync(IReadOnlyCollection<IDataParameter> parameters = null)
        {
            var records = await Context.Db.GetListAsync<TRecord>(parameters?.ToDynamicParameters(), Context.Transaction);
            var entities = records.Adapt<IReadOnlyCollection<TEntity>>();

            return entities;
        }

        public Task<IReadOnlyCollection<TEntity>> GetListAsync(Object parameters = null) => GetListAsync(parameters?.ToParameterCollection<SqlParameter>());

        #endregion
    }
}