using Microsoft.EntityFrameworkCore;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.EntityFramework.Handlers;

public class PhysicalDeletionHandler<TEntity, TIdentifier, TScope> : PhysicalDeletionHandlerBase<TEntity, TIdentifier, TScope?> where TIdentifier : struct, IEquatable<TIdentifier> where TEntity : IEntity<TIdentifier> where TScope : DbContext?
{
    protected override Boolean DoDelete(TEntity entity, TScope? scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var entry = scope.Remove(entity);
        var result = entry.State == EntityState.Deleted;

        return result;
    }

    protected override Task<Boolean> DoDelete(TEntity entity, TScope? scope, CancellationToken cancellationToken) => Task.FromResult(DoDelete(entity, scope));
}
