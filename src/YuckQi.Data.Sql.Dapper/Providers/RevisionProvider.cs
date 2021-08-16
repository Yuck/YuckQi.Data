using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Mapster;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Data.Providers.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.Providers
{
    public class RevisionProvider<TEntity, TKey, TScope, TRecord> : RevisionProviderBase<TEntity, TKey, TScope, TRecord> where TEntity : IEntity<TKey>, IRevised where TKey : struct where TScope : IDbTransaction
    {
        #region Constructors

        public RevisionProvider(RevisionOptions options) : base(options) { }

        #endregion


        #region Protected Methods

        protected override Boolean DoRevise(TEntity entity, TScope scope) => scope.Connection.Update(entity.Adapt<TRecord>(), scope) > 0;

        protected override async Task<Boolean> DoReviseAsync(TEntity entity, TScope scope) => await scope.Connection.UpdateAsync(entity.Adapt<TRecord>(), scope) > 0;

        #endregion
    }
}
