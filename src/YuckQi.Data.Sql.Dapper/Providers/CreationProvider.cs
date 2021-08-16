using System.Data;
using System.Threading.Tasks;
using Dapper;
using Mapster;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.Providers
{
    public class CreationProvider<TEntity, TKey, TScope, TRecord> : CreationProviderBase<TEntity, TKey, TScope, TRecord> where TEntity : IEntity<TKey>, ICreated where TKey : struct where TScope : IDbTransaction
    {
        #region Protected Methods

        protected override TKey? DoCreate(TEntity entity, TScope scope) => scope.Connection.Insert<TKey?, TRecord>(entity.Adapt<TRecord>(), scope);

        protected override Task<TKey?> DoCreateAsync(TEntity entity, TScope scope) => scope.Connection.InsertAsync<TKey?, TRecord>(entity.Adapt<TRecord>(), scope);

        #endregion
    }
}
