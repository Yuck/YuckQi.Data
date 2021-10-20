using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Mapster;
using YuckQi.Data.Filtering;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Sql.Dapper.Extensions;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.Abstract
{
    public class RetrievalProviderBase<TEntity, TKey, TScope, TRecord> : RetrievalHandlerBase<TEntity, TKey, TScope> where TEntity : IEntity<TKey> where TKey : struct where TScope : IDbTransaction
    {
        #region Private Members

        private readonly IReadOnlyDictionary<Type, DbType> _dbTypeMap;
        private readonly ISqlGenerator<TRecord> _sqlGenerator;

        #endregion


        #region Constructors

        protected RetrievalProviderBase(ISqlGenerator<TRecord> sqlGenerator, IReadOnlyDictionary<Type, DbType> dbTypeMap)
        {
            _sqlGenerator = sqlGenerator ?? throw new ArgumentNullException(nameof(sqlGenerator));
            _dbTypeMap = dbTypeMap;
        }

        #endregion


        #region Public Methods

        protected override TEntity DoGet(TKey key, TScope scope)
        {
            var record = scope.Connection.Get<TRecord>(key, scope);
            var entity = record.Adapt<TEntity>();

            return entity;
        }

        protected override async Task<TEntity> DoGetAsync(TKey key, TScope scope)
        {
            var record = await scope.Connection.GetAsync<TRecord>(key, scope);
            var entity = record.Adapt<TEntity>();

            return entity;
        }

        protected override TEntity DoGet(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
        {
            var sql = _sqlGenerator.GenerateGetQuery(parameters);
            var record = scope.Connection.QuerySingleOrDefault<TRecord>(sql, parameters.ToDynamicParameters(_dbTypeMap), scope);
            var entity = record.Adapt<TEntity>();

            return entity;
        }

        protected override async Task<TEntity> DoGetAsync(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
        {
            var sql = _sqlGenerator.GenerateGetQuery(parameters);
            var record = await scope.Connection.QuerySingleOrDefaultAsync<TRecord>(sql, parameters.ToDynamicParameters(_dbTypeMap), scope);
            var entity = record.Adapt<TEntity>();

            return entity;
        }

        protected override IReadOnlyCollection<TEntity> DoGetList(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
        {
            var sql = _sqlGenerator.GenerateGetQuery(parameters);
            var records = scope.Connection.Query<TRecord>(sql, parameters?.ToDynamicParameters(_dbTypeMap), scope);
            var entities = records.Adapt<IReadOnlyCollection<TEntity>>();

            return entities;
        }

        protected override async Task<IReadOnlyCollection<TEntity>> DoGetListAsync(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
        {
            var sql = _sqlGenerator.GenerateGetQuery(parameters);
            var records = await scope.Connection.QueryAsync<TRecord>(sql, parameters?.ToDynamicParameters(_dbTypeMap), scope);
            var entities = records.Adapt<IReadOnlyCollection<TEntity>>();

            return entities;
        }

        #endregion
    }
}
