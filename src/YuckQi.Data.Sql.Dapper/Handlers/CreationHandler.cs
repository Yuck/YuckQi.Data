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
    public class CreationHandler<TEntity, TKey, TScope, TRecord> : CreationHandlerBase<TEntity, TKey, TScope, TRecord> where TEntity : IEntity<TKey>, ICreated where TKey : struct where TScope : IDbTransaction
    {
        #region Constructors

        public CreationHandler(CreationOptions options, IMapper mapper) : base(options, mapper) { }

        #endregion


        #region Protected Methods

        protected override TKey? DoCreate(TEntity entity, TScope scope) => scope.Connection.Insert<TKey?, TRecord>(Mapper.Map<TRecord>(entity), scope);

        protected override Task<TKey?> DoCreateAsync(TEntity entity, TScope scope) => scope.Connection.InsertAsync<TKey?, TRecord>(Mapper.Map<TRecord>(entity), scope);

        #endregion
    }
}
