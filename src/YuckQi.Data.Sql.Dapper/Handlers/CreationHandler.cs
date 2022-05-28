using System.Data;
using System.Threading.Tasks;
using Dapper;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Handlers.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Sql.Dapper.Handlers;

public class CreationHandler<TEntity, TIdentifier, TScope, TRecord> : CreationHandlerBase<TEntity, TIdentifier, TScope, TRecord> where TEntity : IEntity<TIdentifier>, ICreated where TIdentifier : struct where TScope : IDbTransaction
{
    #region Constructors

    public CreationHandler(IMapper mapper) : base(mapper) { }

    public CreationHandler(IMapper mapper, CreationOptions options) : base(mapper, options) { }

    #endregion


    #region Protected Methods

    protected override TIdentifier? DoCreate(TEntity entity, TScope scope) => scope.Connection.Insert<TIdentifier?, TRecord>(Mapper.Map<TRecord>(entity), scope);

    protected override Task<TIdentifier?> DoCreateAsync(TEntity entity, TScope scope) => scope.Connection.InsertAsync<TIdentifier?, TRecord>(Mapper.Map<TRecord>(entity), scope);

    #endregion
}
