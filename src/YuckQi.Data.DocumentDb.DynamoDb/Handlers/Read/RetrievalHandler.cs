using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using YuckQi.Data.DocumentDb.DynamoDb.Extensions;
using YuckQi.Data.Filtering;
using YuckQi.Data.Handlers.Read.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.DocumentDb.DynamoDb.Handlers.Read;

public class RetrievalHandler<TEntity, TIdentifier, TScope> : RetrievalHandler<TEntity, TIdentifier, TScope?, TEntity> where TEntity : IEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier> where TScope : IDynamoDBContext?
{
    public RetrievalHandler(Func<TIdentifier, Primitive> hashKeyValueFactory) : base(hashKeyValueFactory, null) { }

    public RetrievalHandler(Func<TIdentifier, Primitive> hashKeyValueFactory, Func<TIdentifier, Primitive>? rangeKeyValueFactory) : base(hashKeyValueFactory, rangeKeyValueFactory, null) { }
}

public class RetrievalHandler<TEntity, TIdentifier, TScope, TDocument> : RetrievalHandlerBase<TEntity, TIdentifier, TScope?, TDocument> where TEntity : IEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier> where TScope : IDynamoDBContext?
{
    private readonly Func<TIdentifier, Primitive> _hashKeyValueFactory;
    private readonly Func<TIdentifier, Primitive>? _rangeKeyValueFactory;

    public RetrievalHandler(Func<TIdentifier, Primitive> hashKeyValueFactory, IMapper? mapper) : this(hashKeyValueFactory, null, mapper) { }

    public RetrievalHandler(Func<TIdentifier, Primitive> hashKeyValueFactory, Func<TIdentifier, Primitive>? rangeKeyValueFactory, IMapper? mapper) : base(mapper)
    {
        _hashKeyValueFactory = hashKeyValueFactory ?? throw new ArgumentNullException(nameof(hashKeyValueFactory));
        _rangeKeyValueFactory = rangeKeyValueFactory ?? throw new ArgumentNullException(nameof(rangeKeyValueFactory));
    }

    protected override TEntity? DoGet(TIdentifier key, TScope? scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var task = Task.Run(async () => await DoGet(key, scope, default));
        var result = task.Result;

        return result;
    }

    protected override async Task<TEntity?> DoGet(TIdentifier identifier, TScope? scope, CancellationToken cancellationToken)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var table = scope.GetTargetTable<TDocument>();
        var hashKey = _hashKeyValueFactory(identifier);
        var rangeKey = _rangeKeyValueFactory?.Invoke(identifier);
        var result = rangeKey != null
                         ? await table.GetItemAsync(hashKey, rangeKey, cancellationToken)
                         : await table.GetItemAsync(hashKey, cancellationToken);
        var document = scope.FromDocument<TDocument>(result);
        var entity = MapToEntity(document);

        return entity;
    }

    protected override TEntity? DoGet(IReadOnlyCollection<FilterCriteria> parameters, TScope? scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var task = Task.Run(async () => await DoGet(parameters, scope, default));
        var result = task.Result;

        return result;
    }

    protected override async Task<TEntity?> DoGet(IReadOnlyCollection<FilterCriteria> parameters, TScope? scope, CancellationToken cancellationToken)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var table = scope.GetTargetTable<TDocument>();
        var filter = parameters.ToQueryFilter();
        var search = table.Query(filter) as Search;
        var documents = await GetDocuments(scope, search, cancellationToken);
        var document = documents.SingleOrDefault();
        var entity = MapToEntity(document);

        return entity;
    }

    protected override IReadOnlyCollection<TEntity> DoGetList(IReadOnlyCollection<FilterCriteria>? parameters, TScope? scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var task = Task.Run(async () => await DoGetList(parameters, scope, default));
        var result = task.Result;

        return result;
    }

    protected override async Task<IReadOnlyCollection<TEntity>> DoGetList(IReadOnlyCollection<FilterCriteria>? parameters, TScope? scope, CancellationToken cancellationToken)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var table = scope.GetTargetTable<TDocument>();
        var filter = parameters?.ToQueryFilter();
        var search = table.Query(filter) as Search;
        var documents = await GetDocuments(scope, search, cancellationToken);
        var entities = MapToEntityCollection(documents);

        return entities;
    }

    private static async Task<IEnumerable<TDocument>> GetDocuments(TScope? scope, Search? search, CancellationToken cancellationToken)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));
        if (search == null)
            throw new ArgumentNullException(nameof(search));

        var documents = new List<Document>();

        while (! search.IsDone)
            documents.AddRange(await search.GetNextSetAsync(cancellationToken));
        documents.AddRange(await search.GetRemainingAsync(cancellationToken));

        return scope.FromDocuments<TDocument>(documents);
    }
}
