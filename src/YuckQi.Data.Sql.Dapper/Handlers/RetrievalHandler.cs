using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Mapster;
using YuckQi.Data.Abstract;
using YuckQi.Data.Extensions;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Sql.Dapper.Handlers.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.Handlers
{
    public class RetrievalHandler<TEntity, TKey, TRecord> : ReadHandlerBase<TRecord>, IRetrievalHandler<TEntity, TKey> where TEntity : IEntity<TKey> where TKey : struct
    {
        #region Constructors

        public RetrievalHandler(IUnitOfWork uow) : base(uow)
        {
        }

        #endregion


        #region Public Methods

        public async Task<TEntity> GetAsync(TKey key)
        {
            var record = await Db.GetAsync<TRecord>(key, Transaction);
            var entity = record.Adapt<TEntity>();

            return entity;
        }

        public async Task<TEntity> GetAsync(IReadOnlyCollection<IDataParameter> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            var sql = BuildParameterizedSql(parameters);
            var record = await Db.QuerySingleOrDefaultAsync<TRecord>(sql, parameters, Transaction);
            var entity = record.Adapt<TEntity>();

            return entity;
        }

        public Task<TEntity> GetAsync(object parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            return GetAsync(parameters.ToParameterCollection<SqlParameter>());
        }

        public async Task<IReadOnlyCollection<TEntity>> GetListAsync(IReadOnlyCollection<IDataParameter> parameters = null)
        {
            var records = await Db.GetListAsync<TRecord>(parameters, Transaction);
            var entities = records.Adapt<IReadOnlyCollection<TEntity>>();

            return entities;
        }

        public Task<IReadOnlyCollection<TEntity>> GetListAsync(object parameters = null)
        {
            return GetListAsync(parameters?.ToParameterCollection<SqlParameter>());
        }

        #endregion
    }
}