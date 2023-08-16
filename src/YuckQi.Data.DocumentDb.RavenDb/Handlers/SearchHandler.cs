using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using YuckQi.Data.Filtering;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Sorting;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Domain.ValueObjects.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.DocumentDb.RavenDb.Handlers;

public class SearchHandler<TEntity, TIdentifier, TScope> : SearchHandler<TEntity, TIdentifier, TScope?, TEntity> where TEntity : IEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier> where TScope : IDocumentSession?
{
    public SearchHandler() : base(null) { }
}

public class SearchHandler<TEntity, TIdentifier, TScope, TDocument> : SearchHandlerBase<TEntity, TIdentifier, TScope?> where TEntity : IEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier> where TScope : IDocumentSession?
{
    public SearchHandler(IMapper? mapper) : base(mapper) { }

    protected override Int32 DoCount(IReadOnlyCollection<FilterCriteria> parameters, TScope? scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var query = scope.Query<TDocument>().Where(document => parameters.Select(t => t.ToExpression(document)).All(t => t()));
        var count = query.Count();

        return count;
    }

    protected override async Task<Int32> DoCount(IReadOnlyCollection<FilterCriteria> parameters, TScope? scope, CancellationToken cancellationToken)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var query = scope.Query<TDocument>().Where(document => parameters.Select(t => t.ToExpression(document)).All(t => t()));
        var count = await query.CountAsync(cancellationToken);

        return count;
    }

    protected override IReadOnlyCollection<TEntity> DoSearch(IReadOnlyCollection<FilterCriteria> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, TScope? scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var query = scope.Query<TDocument>().Where(document => parameters.Select(t => t.ToExpression(document)).All(t => t()));
        var documents = query.ToList();
        var entities = MapToEntityCollection(documents);

        return entities;
    }

    protected override async Task<IReadOnlyCollection<TEntity>> DoSearch(IReadOnlyCollection<FilterCriteria> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, TScope? scope, CancellationToken cancellationToken)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var query = scope.Query<TDocument>().Where(document => parameters.Select(t => t.ToExpression(document)).All(t => t()));
        var documents = await query.ToListAsync(cancellationToken);
        var entities = MapToEntityCollection(documents);

        return entities;
    }
}
