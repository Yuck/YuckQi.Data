using Microsoft.EntityFrameworkCore;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.EntityFramework.Handlers;

public class CreationHandler<TEntity, TIdentifier, TScope> : CreationHandlerBase<TEntity, TIdentifier, TScope> where TIdentifier : struct where TEntity : class, IEntity<TIdentifier>, ICreated where TScope : DbContext
{
    protected override TIdentifier? DoCreate(TEntity entity, TScope scope)
    {
        var entry = scope.Add(entity);
        var identifier = entry.Entity.Identifier;

        return identifier;
    }

    protected override async Task<TIdentifier?> DoCreate(TEntity entity, TScope scope, CancellationToken cancellationToken)
    {
        var entry = await scope.AddAsync(entity, cancellationToken);
        var identifier = entry.Entity.Identifier;

        return identifier;
    }
}
