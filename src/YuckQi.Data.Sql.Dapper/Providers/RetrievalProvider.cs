using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Mapster;
using YuckQi.Data.Extensions;
using YuckQi.Data.Filtering;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Data.Sql.Dapper.Abstract;
using YuckQi.Data.Sql.Dapper.Extensions;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.Providers
{
    public class RetrievalProvider<TEntity, TKey, TRecord, TScope> : IRetrievalProvider<TEntity, TKey, TScope> where TEntity : IEntity<TKey> where TKey : struct where TScope : IDbTransaction
    {
        #region Private Members

        private readonly ISqlGenerator<TRecord> _sqlGenerator;

        #endregion


        #region Constructors

        public RetrievalProvider(ISqlGenerator<TRecord> sqlGenerator)
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

        public TEntity Get(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
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

        public async Task<TEntity> GetAsync(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
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

            return Get(parameters.ToFilterCollection(), scope);
        }

        public Task<TEntity> GetAsync(Object parameters, TScope scope)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            return GetAsync(parameters.ToFilterCollection(), scope);
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

        public IReadOnlyCollection<TEntity> GetList(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            return DoGetList(null, scope);
        }

        public Task<IReadOnlyCollection<TEntity>> GetListAsync(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            return DoGetListAsync(null, scope);
        }

        public IReadOnlyCollection<TEntity> GetList(Object parameters, TScope scope) => GetList(parameters?.ToFilterCollection(), scope);

        public Task<IReadOnlyCollection<TEntity>> GetListAsync(Object parameters, TScope scope) => GetListAsync(parameters?.ToFilterCollection(), scope);

        #endregion


        #region Supporting Methods

        private static IReadOnlyCollection<TEntity> DoGetList(IEnumerable<FilterCriteria> parameters, TScope scope)
        {
            var records = scope.Connection.GetList<TRecord>(parameters?.ToDynamicParameters(), scope);
            var entities = records.Adapt<IReadOnlyCollection<TEntity>>();

            return entities;
        }

        private static async Task<IReadOnlyCollection<TEntity>> DoGetListAsync(IEnumerable<FilterCriteria> parameters, TScope scope)
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
