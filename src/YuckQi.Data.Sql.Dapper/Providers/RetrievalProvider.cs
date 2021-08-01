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
    public class RetrievalProvider<TEntity, TKey, TRecord, TScope, TDataParameter> : IRetrievalProvider<TEntity, TKey, TScope, TDataParameter> where TEntity : IEntity<TKey> where TKey : struct where TScope : IDbTransaction where TDataParameter : IDataParameter, new()
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

        public TEntity Get(TKey key, TScope scope)
        {
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            var record = scope.Connection.Get<TRecord>(key, scope);
            var entity = record.Adapt<TEntity>();

            return entity;
        }

        public async Task<TEntity> GetAsync(TKey key, TScope scope)
        {
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            var record = await scope.Connection.GetAsync<TRecord>(key, scope);
            var entity = record.Adapt<TEntity>();

            return entity;
        }

        public TEntity Get(IReadOnlyCollection<TDataParameter> parameters, TScope scope)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            var sql = _sqlGenerator.GenerateGetQuery(parameters);
            var record = scope.Connection.QuerySingleOrDefault<TRecord>(sql, parameters.ToDynamicParameters(), scope);
            var entity = record.Adapt<TEntity>();

            return entity;
        }

        public async Task<TEntity> GetAsync(IReadOnlyCollection<TDataParameter> parameters, TScope scope)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            var sql = _sqlGenerator.GenerateGetQuery(parameters);
            var record = await scope.Connection.QuerySingleOrDefaultAsync<TRecord>(sql, parameters.ToDynamicParameters(), scope);
            var entity = record.Adapt<TEntity>();

            return entity;
        }

        public TEntity Get(Object parameters, TScope scope)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            return Get(parameters.ToParameterCollection<TDataParameter>(), scope);
        }

        public Task<TEntity> GetAsync(Object parameters, TScope scope)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            return GetAsync(parameters.ToParameterCollection<TDataParameter>(), scope);
        }

        public IReadOnlyCollection<TEntity> GetList(TScope scope)
        {
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            return DoGetList(null, scope);
        }

        public Task<IReadOnlyCollection<TEntity>> GetListAsync(TScope scope)
        {
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            return DoGetListAsync(null, scope);
        }

        public IReadOnlyCollection<TEntity> GetList(IReadOnlyCollection<TDataParameter> parameters, TScope scope)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            return DoGetList(null, scope);
        }

        public Task<IReadOnlyCollection<TEntity>> GetListAsync(IReadOnlyCollection<TDataParameter> parameters, TScope scope)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            return DoGetListAsync(null, scope);
        }

        public IReadOnlyCollection<TEntity> GetList(Object parameters, TScope scope) => GetList(parameters?.ToParameterCollection<TDataParameter>(), scope);
        public Task<IReadOnlyCollection<TEntity>> GetListAsync(Object parameters, TScope scope) => GetListAsync(parameters?.ToParameterCollection<TDataParameter>(), scope);

        #endregion


        #region Supporting Methods

        private static IReadOnlyCollection<TEntity> DoGetList(IEnumerable<TDataParameter> parameters, TScope scope)
        {
            var records = scope.Connection.GetList<TRecord>(parameters?.ToDynamicParameters(), scope);
            var entities = records.Adapt<IReadOnlyCollection<TEntity>>();

            return entities;
        }

        private static async Task<IReadOnlyCollection<TEntity>> DoGetListAsync(IEnumerable<TDataParameter> parameters, TScope scope)
        {
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            var records = await scope.Connection.GetListAsync<TRecord>(parameters?.ToDynamicParameters(), scope);
            var entities = records.Adapt<IReadOnlyCollection<TEntity>>();

            return entities;
        }

        #endregion
    }
}