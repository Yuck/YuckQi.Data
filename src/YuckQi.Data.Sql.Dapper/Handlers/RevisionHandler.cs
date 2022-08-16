using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Handlers.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Sql.Dapper.Handlers;

public class RevisionHandler<TEntity, TIdentifier, TScope, TRecord> : RevisionHandlerBase<TEntity, TIdentifier, TScope, TRecord> where TEntity : IEntity<TIdentifier>, IRevised where TIdentifier : struct where TScope : IDbTransaction
{
    #region Constructors

    public RevisionHandler(IMapper mapper) : base(mapper) { }

    public RevisionHandler(IMapper mapper, RevisionOptions options) : base(mapper, options) { }

    #endregion


    #region Protected Methods

    protected override Boolean DoRevise(TEntity entity, TScope scope) => scope.Connection.Update(Mapper.Map<TRecord>(entity), scope) > 0;

    protected override async Task<Boolean> DoRevise(TEntity entity, TScope scope, CancellationToken cancellationToken) => await scope.Connection.UpdateAsync(Mapper.Map<TRecord>(entity), scope, token: cancellationToken) > 0;

    #endregion
}
