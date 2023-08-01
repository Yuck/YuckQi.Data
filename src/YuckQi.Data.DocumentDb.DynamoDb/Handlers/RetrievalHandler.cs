using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using YuckQi.Data.DocumentDb.DynamoDb.Extensions;
using YuckQi.Data.Filtering;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.DocumentDb.DynamoDb.Handlers;

public class RetrievalHandler<TEntity, TIdentifier, TScope, TDocument> : RetrievalHandlerBase<TEntity, TIdentifier, TScope?> where TEntity : IEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier> where TScope : IDynamoDBContext?
{
    private readonly Func<TIdentifier, Primitive> _hashKeyValueFactory;
    private readonly Func<TIdentifier, Primitive>? _rangeKeyValueFactory;

    public RetrievalHandler(Func<TIdentifier, Primitive> hashKeyValueFactory, IMapper mapper) : this(hashKeyValueFactory, null, mapper) { }

    public RetrievalHandler(Func<TIdentifier, Primitive> hashKeyValueFactory, Func<TIdentifier, Primitive>? rangeKeyValueFactory, IMapper mapper) : base(mapper)
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
        var search = table.Query(filter);
        var results = await search.GetRemainingAsync(cancellationToken);
        var result = results.SingleOrDefault();
        var document = scope.FromDocument<TDocument>(result);
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
        var filter = parameters?.ToScanFilter();
        var search = table.Scan(filter);
        var results = await search.GetRemainingAsync(cancellationToken);
        var documents = scope.FromDocuments<TDocument>(results);
        var entities = MapToEntityCollection(documents);

        return entities;
    }
}
