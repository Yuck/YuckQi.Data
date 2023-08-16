using CSharpFunctionalExtensions;
using Raven.Client.Documents.Session;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Handlers.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.DocumentDb.RavenDb.Handlers;

public class CreationHandler<TEntity, TIdentifier, TScope> : CreationHandler<TEntity, TIdentifier, TScope?, TEntity> where TEntity : IEntity<TIdentifier>, ICreated where TIdentifier : struct, IEquatable<TIdentifier> where TScope : IDocumentSession?
{
    public CreationHandler() : this(null) { }

    public CreationHandler(CreationOptions<TIdentifier>? options) : base(options, null) { }
}

public class CreationHandler<TEntity, TIdentifier, TScope, TDocument> : CreationHandlerBase<TEntity, TIdentifier, TScope?> where TEntity : IEntity<TIdentifier>, ICreated where TIdentifier : IEquatable<TIdentifier> where TScope : IDocumentSession?
{
    public CreationHandler(IMapper? mapper) : base(mapper) { }

    public CreationHandler(CreationOptions<TIdentifier>? options, IMapper? mapper) : base(options, mapper) { }

    public override IEnumerable<TEntity> Create(IEnumerable<TEntity> entities, TScope? scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var list = entities.ToList();

        foreach (var entity in list)
            DoCreate(entity, scope);

        return list;
    }

    public override Task<IEnumerable<TEntity>> Create(IEnumerable<TEntity> entities, TScope? scope, CancellationToken cancellationToken) => Task.FromResult(Create(entities, scope));

    protected override Maybe<TIdentifier?> DoCreate(TEntity entity, TScope? scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        scope.Store(MapToData<TDocument>(entity) ?? throw new NullReferenceException());

        return Maybe<TIdentifier?>.From(entity.Identifier);
    }

    protected override Task<Maybe<TIdentifier?>> DoCreate(TEntity entity, TScope? scope, CancellationToken cancellationToken) => Task.FromResult(DoCreate(entity, scope));
}
