using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Handlers.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Sql.Dapper.Handlers
{
    public class RevisionHandler<TEntity, TKey, TScope, TRecord> : RevisionHandlerBase<TEntity, TKey, TScope, TRecord> where TEntity : IEntity<TKey>, IRevised where TKey : struct where TScope : IDbTransaction
    {
        #region Constructors

        public RevisionHandler(RevisionOptions options, IMapper mapper) : base(options, mapper) { }

        #endregion


        #region Protected Methods

        protected override Boolean DoRevise(TEntity entity, TScope scope) => scope.Connection.Update(Mapper.Map<TRecord>(entity), scope) > 0;

        protected override async Task<Boolean> DoReviseAsync(TEntity entity, TScope scope) => await scope.Connection.UpdateAsync(Mapper.Map<TRecord>(entity), scope) > 0;

        #endregion
    }
}
