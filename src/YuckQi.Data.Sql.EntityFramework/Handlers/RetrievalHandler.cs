using Microsoft.EntityFrameworkCore;
using YuckQi.Data.Filtering;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.EntityFramework.Handlers;

public class RetrievalHandler<TEntity, TIdentifier, TScope> : RetrievalHandlerBase<TEntity, TIdentifier, TScope> where TIdentifier : struct where TEntity : class, IEntity<TIdentifier> where TScope : DbContext
{
    public RetrievalHandler() : base(null) { }

    protected override TEntity? DoGet(TIdentifier identifier, TScope scope) => scope.Find<TEntity>(identifier); // TODO: scope.Set<TEntity>().SingleOrDefault(t => t.Identifier == identifier)

    protected override async Task<TEntity?> DoGet(TIdentifier identifier, TScope scope, CancellationToken cancellationToken) => await scope.FindAsync<TEntity>(identifier);

    protected override TEntity? DoGet(IReadOnlyCollection<FilterCriteria> parameters, TScope scope) => throw new NotImplementedException();

    protected override Task<TEntity?> DoGet(IReadOnlyCollection<FilterCriteria> parameters, TScope scope, CancellationToken cancellationToken) => throw new NotImplementedException();

    protected override IReadOnlyCollection<TEntity> DoGetList(IReadOnlyCollection<FilterCriteria>? parameters, TScope scope) => throw new NotImplementedException();

    protected override Task<IReadOnlyCollection<TEntity>> DoGetList(IReadOnlyCollection<FilterCriteria>? parameters, TScope scope, CancellationToken cancellationToken) => throw new NotImplementedException();
}
