using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using YuckQi.Data.Filtering;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.DocumentDb.RavenDb.Handlers;

public class RetrievalHandler<TEntity, TIdentifier, TScope> : RetrievalHandler<TEntity, TIdentifier, TScope?, TEntity> where TEntity : IEntity<TIdentifier> where TIdentifier : struct, IEquatable<TIdentifier> where TScope : IDocumentSession?
{
    public RetrievalHandler() : base(null) { }

    public RetrievalHandler(Func<TIdentifier, String> identifierConverter) : base(identifierConverter, null) { }
}

public class RetrievalHandler<TEntity, TIdentifier, TScope, TDocument> : RetrievalHandlerBase<TEntity, TIdentifier, TScope?> where TEntity : IEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier> where TScope : IDocumentSession?
{
    private readonly Func<TIdentifier, String> _identifierConverter;

    public RetrievalHandler(IMapper? mapper) : this(identifier => $"{identifier}", mapper) { }

    public RetrievalHandler(Func<TIdentifier, String> identifierConverter, IMapper? mapper) : base(mapper)
    {
        _identifierConverter = identifierConverter;
    }

    protected override TEntity? DoGet(TIdentifier identifier, TScope? scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var id = _identifierConverter(identifier);
        var document = scope.Load<TDocument>(id);
        var entity = MapToEntity(document);

        return entity;
    }

    protected override Task<TEntity?> DoGet(TIdentifier identifier, TScope? scope, CancellationToken cancellationToken) => Task.FromResult(DoGet(identifier, scope));

    protected override TEntity? DoGet(IReadOnlyCollection<FilterCriteria> parameters, TScope? scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var query = scope.Query<TDocument>().Where(document => parameters.Select(t => t.ToExpression(document)).All(t => t()));
        var documents = query.ToList();
        var document = documents.SingleOrDefault();
        var entity = MapToEntity(document);

        return entity;
    }

    protected override async Task<TEntity?> DoGet(IReadOnlyCollection<FilterCriteria> parameters, TScope? scope, CancellationToken cancellationToken)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var query = scope.Query<TDocument>().Where(document => parameters.Select(t => t.ToExpression(document)).All(t => t()));
        var documents = await query.ToListAsync(cancellationToken);
        var document = documents.SingleOrDefault();
        var entity = MapToEntity(document);

        return entity;
    }

    protected override IReadOnlyCollection<TEntity> DoGetList(IReadOnlyCollection<FilterCriteria>? parameters, TScope? scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var query = parameters != null
                        ? scope.Query<TDocument>().Where(document => parameters.Select(t => t.ToExpression(document)).All(t => t()))
                        : scope.Query<TDocument>();
        var documents = query.ToList();
        var entities = MapToEntityCollection(documents);

        return entities;
    }

    protected override async Task<IReadOnlyCollection<TEntity>> DoGetList(IReadOnlyCollection<FilterCriteria>? parameters, TScope? scope, CancellationToken cancellationToken)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var query = parameters != null
                        ? scope.Query<TDocument>().Where(document => parameters.Select(t => t.ToExpression(document)).All(t => t()))
                        : scope.Query<TDocument>();
        var documents = await query.ToListAsync(cancellationToken);
        var entities = MapToEntityCollection(documents);

        return entities;
    }
}
