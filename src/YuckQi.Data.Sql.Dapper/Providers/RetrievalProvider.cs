﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Mapster;
using YuckQi.Data.Abstract;
using YuckQi.Data.Extensions;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Data.Sql.Dapper.Abstract;
using YuckQi.Data.Sql.Dapper.Extensions;
using YuckQi.Data.Sql.Dapper.Providers.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.Providers
{
    public class RetrievalProvider<TEntity, TKey, TRecord, TDataParameter> : DataProviderBase, IRetrievalProvider<TEntity, TKey, TDataParameter> where TEntity : IEntity<TKey> where TKey : struct where TDataParameter : IDataParameter, new()
    {
        #region Private Members

        private readonly ISqlGenerator<TRecord, TDataParameter> _sqlGenerator;

        #endregion


        #region Constructors

        public RetrievalProvider(IUnitOfWork context, ISqlGenerator<TRecord, TDataParameter> sqlGenerator) : base(context)
        {
            _sqlGenerator = sqlGenerator ?? throw new ArgumentNullException(nameof(sqlGenerator));
        }

        #endregion


        #region Public Methods

        public async Task<TEntity> GetAsync(TKey key)
        {
            var record = await Context.Db.GetAsync<TRecord>(key, Context.Transaction);
            var entity = record.Adapt<TEntity>();

            return entity;
        }

        public async Task<TEntity> GetAsync(IReadOnlyCollection<TDataParameter> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            var sql = _sqlGenerator.GenerateGetQuery(parameters);
            var record = await Context.Db.QuerySingleOrDefaultAsync<TRecord>(sql, parameters.ToDynamicParameters(), Context.Transaction);
            var entity = record.Adapt<TEntity>();

            return entity;
        }

        public Task<TEntity> GetAsync(Object parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            return GetAsync(parameters.ToParameterCollection<TDataParameter>());
        }

        public async Task<IReadOnlyCollection<TEntity>> GetListAsync(IReadOnlyCollection<TDataParameter> parameters = null)
        {
            var records = await Context.Db.GetListAsync<TRecord>(parameters?.ToDynamicParameters(), Context.Transaction);
            var entities = records.Adapt<IReadOnlyCollection<TEntity>>();

            return entities;
        }

        public Task<IReadOnlyCollection<TEntity>> GetListAsync(Object parameters = null) => GetListAsync(parameters?.ToParameterCollection<TDataParameter>());

        #endregion
    }
}