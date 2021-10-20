using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Mapster;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.Providers
{
    public class PhysicalDeletionProvider<TEntity, TKey, TScope, TRecord> : PhysicalDeletionHandlerBase<TEntity, TKey, TScope, TRecord> where TEntity : IEntity<TKey> where TKey : struct where TScope : IDbTransaction
    {
        #region Protected Methods

        protected override Boolean DoDelete(TEntity entity, TScope scope) => scope.Connection.Delete(entity.Adapt<TRecord>(), scope) > 0;

        protected override async Task<Boolean> DoDeleteAsync(TEntity entity, TScope scope) => await scope.Connection.DeleteAsync(entity.Adapt<TRecord>(), scope) > 0;

        #endregion
    }
}
