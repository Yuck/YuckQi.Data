using Raven.Client.Documents.Session;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.DocumentDb.RavenDb.Handlers;

public class PhysicalDeletionHandler<TEntity, TIdentifier, TScope> : PhysicalDeletionHandler<TEntity, TIdentifier, TScope?, TEntity> where TEntity : IEntity<TIdentifier> where TIdentifier : struct, IEquatable<TIdentifier> where TScope : IDocumentSession?
{
    public PhysicalDeletionHandler() : base(null) { }
}

public class PhysicalDeletionHandler<TEntity, TIdentifier, TScope, TDocument> : PhysicalDeletionHandlerBase<TEntity, TIdentifier, TScope?> where TEntity : IEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier> where TScope : IDocumentSession?
{
    public PhysicalDeletionHandler(IMapper? mapper) : base(mapper) { }

    protected override Boolean DoDelete(TEntity entity, TScope? scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var document = MapToData<TDocument>(entity) ?? throw new NullReferenceException();

        scope.Delete(document);

        return true;
    }

    protected override Task<Boolean> DoDelete(TEntity entity, TScope? scope, CancellationToken cancellationToken) => Task.FromResult(DoDelete(entity, scope));
}
