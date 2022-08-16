using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Sql.Dapper.Handlers;

public class PhysicalDeletionHandler<TEntity, TIdentifier, TScope, TRecord> : PhysicalDeletionHandlerBase<TEntity, TIdentifier, TScope, TRecord> where TEntity : IEntity<TIdentifier> where TIdentifier : struct where TScope : IDbTransaction
{
    #region Constructors

    public PhysicalDeletionHandler(IMapper mapper) : base(mapper) { }

    #endregion


    #region Protected Methods

    protected override Boolean DoDelete(TEntity entity, TScope scope) => scope.Connection.Delete(Mapper.Map<TRecord>(entity), scope) > 0;

    protected override async Task<Boolean> DoDelete(TEntity entity, TScope scope, CancellationToken cancellationToken) => await scope.Connection.DeleteAsync(Mapper.Map<TRecord>(entity), scope) > 0;

    #endregion
}
