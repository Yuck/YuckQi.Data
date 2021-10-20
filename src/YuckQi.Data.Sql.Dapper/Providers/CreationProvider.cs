﻿using System.Data;
using System.Threading.Tasks;
using Dapper;
using Mapster;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Handlers.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.Providers
{
    public class CreationProvider<TEntity, TKey, TScope, TRecord> : CreationHandlerBase<TEntity, TKey, TScope, TRecord> where TEntity : IEntity<TKey>, ICreated where TKey : struct where TScope : IDbTransaction
    {
        #region Constructors

        public CreationProvider(CreationOptions options) : base(options) { }

        #endregion


        #region Protected Methods

        protected override TKey? DoCreate(TEntity entity, TScope scope) => scope.Connection.Insert<TKey?, TRecord>(entity.Adapt<TRecord>(), scope);

        protected override Task<TKey?> DoCreateAsync(TEntity entity, TScope scope) => scope.Connection.InsertAsync<TKey?, TRecord>(entity.Adapt<TRecord>(), scope);

        #endregion
    }
}
