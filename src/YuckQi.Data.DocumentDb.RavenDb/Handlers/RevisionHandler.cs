using Raven.Client.Documents.Session;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Handlers.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.DocumentDb.RavenDb.Handlers;

public class RevisionHandler<TEntity, TIdentifier, TScope> : RevisionHandler<TEntity, TIdentifier, TScope?, TEntity> where TEntity : IEntity<TIdentifier>, IRevised where TIdentifier : struct, IEquatable<TIdentifier> where TScope : IDocumentSession?
{
    public RevisionHandler() : this(null) { }

    public RevisionHandler(RevisionOptions? options) : base(options, null) { }
}

public class RevisionHandler<TEntity, TIdentifier, TScope, TDocument> : RevisionHandlerBase<TEntity, TIdentifier, TScope?> where TEntity : IEntity<TIdentifier>, IRevised where TIdentifier : IEquatable<TIdentifier> where TScope : IDocumentSession?
{
    public RevisionHandler(IMapper? mapper) : base(mapper) { }

    public RevisionHandler(RevisionOptions? options, IMapper? mapper) : base(options, mapper) { }

    public override IEnumerable<TEntity> Revise(IEnumerable<TEntity> entities, TScope? scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var list = entities.ToList();

        foreach (var entity in list)
            DoRevise(entity, scope);

        return list;
    }

    public override Task<IEnumerable<TEntity>> Revise(IEnumerable<TEntity> entities, TScope? scope, CancellationToken cancellationToken) => Task.FromResult(Revise(entities, scope));

    protected override Boolean DoRevise(TEntity entity, TScope? scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        scope.Store(MapToData<TDocument>(entity) ?? throw new NullReferenceException());

        return true;
    }

    protected override Task<Boolean> DoRevise(TEntity entity, TScope? scope, CancellationToken cancellationToken) => Task.FromResult(DoRevise(entity, scope));
}
