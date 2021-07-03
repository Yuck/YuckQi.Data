using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Mapster;
using YuckQi.Data.Extensions;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Data.Sql.Dapper.Abstract;
using YuckQi.Data.Sql.Dapper.Extensions;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.Providers
{
    public class RetrievalProvider<TEntity, TKey, TRecord, TDataParameter> : IRetrievalProvider<TEntity, TKey, TDataParameter> where TEntity : IEntity<TKey> where TKey : struct where TDataParameter : IDataParameter, new()
    {
        #region Private Members

        private readonly ISqlGenerator<TRecord, TDataParameter> _sqlGenerator;

        #endregion


        #region Constructors

        public RetrievalProvider(ISqlGenerator<TRecord, TDataParameter> sqlGenerator)
        {
            _sqlGenerator = sqlGenerator ?? throw new ArgumentNullException(nameof(sqlGenerator));
        }

        #endregion


        #region Public Methods

        public TEntity Get(TKey key, IDbTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            var record = transaction.Connection.Get<TRecord>(key, transaction);
            var entity = record.Adapt<TEntity>();

            return entity;
        }

        public async Task<TEntity> GetAsync(TKey key, IDbTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            var record = await transaction.Connection.GetAsync<TRecord>(key, transaction);
            var entity = record.Adapt<TEntity>();

            return entity;
        }

        public TEntity Get(IReadOnlyCollection<TDataParameter> parameters, IDbTransaction transaction)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            var sql = _sqlGenerator.GenerateGetQuery(parameters);
            var record = transaction.Connection.QuerySingleOrDefault<TRecord>(sql, parameters.ToDynamicParameters(), transaction);
            var entity = record.Adapt<TEntity>();

            return entity;
        }

        public async Task<TEntity> GetAsync(IReadOnlyCollection<TDataParameter> parameters, IDbTransaction transaction)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            var sql = _sqlGenerator.GenerateGetQuery(parameters);
            var record = await transaction.Connection.QuerySingleOrDefaultAsync<TRecord>(sql, parameters.ToDynamicParameters(), transaction);
            var entity = record.Adapt<TEntity>();

            return entity;
        }

        public TEntity Get(Object parameters, IDbTransaction transaction)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            return Get(parameters.ToParameterCollection<TDataParameter>(), transaction);
        }

        public Task<TEntity> GetAsync(Object parameters, IDbTransaction transaction)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            return GetAsync(parameters.ToParameterCollection<TDataParameter>(), transaction);
        }

        public IReadOnlyCollection<TEntity> GetList(IDbTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            return DoGetList(null, transaction);
        }

        public Task<IReadOnlyCollection<TEntity>> GetListAsync(IDbTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            return DoGetListAsync(null, transaction);
        }

        public IReadOnlyCollection<TEntity> GetList(IReadOnlyCollection<TDataParameter> parameters, IDbTransaction transaction)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            return DoGetList(null, transaction);
        }

        public Task<IReadOnlyCollection<TEntity>> GetListAsync(IReadOnlyCollection<TDataParameter> parameters, IDbTransaction transaction)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            return DoGetListAsync(null, transaction);
        }

        public IReadOnlyCollection<TEntity> GetList(Object parameters, IDbTransaction transaction) => GetList(parameters?.ToParameterCollection<TDataParameter>(), transaction);
        public Task<IReadOnlyCollection<TEntity>> GetListAsync(Object parameters, IDbTransaction transaction) => GetListAsync(parameters?.ToParameterCollection<TDataParameter>(), transaction);

        #endregion


        #region Supporting Methods

        private static IReadOnlyCollection<TEntity> DoGetList(IEnumerable<TDataParameter> parameters, IDbTransaction transaction)
        {
            var records = transaction.Connection.GetList<TRecord>(parameters?.ToDynamicParameters(), transaction);
            var entities = records.Adapt<IReadOnlyCollection<TEntity>>();

            return entities;
        }

        public async Task<IReadOnlyCollection<TEntity>> DoGetListAsync(IReadOnlyCollection<TDataParameter> parameters, IDbTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            var records = await transaction.Connection.GetListAsync<TRecord>(parameters?.ToDynamicParameters(), transaction);
            var entities = records.Adapt<IReadOnlyCollection<TEntity>>();

            return entities;
        }

        #endregion
    }
}