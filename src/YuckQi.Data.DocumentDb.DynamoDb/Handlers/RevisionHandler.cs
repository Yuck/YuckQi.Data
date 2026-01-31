using Amazon.DynamoDBv2.DataModel;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Handlers.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.DocumentDb.DynamoDb.Handlers;

public class RevisionHandler<TEntity, TIdentifier, TScope> : RevisionHandler<TEntity, TIdentifier, TScope?, TEntity> where TEntity : IEntity<TIdentifier>, IRevised where TIdentifier : IEquatable<TIdentifier> where TScope : IDynamoDBContext?
{
    public RevisionHandler() : this(null) { }

    public RevisionHandler(RevisionOptions? options) : base(options, null) { }
}

public class RevisionHandler<TEntity, TIdentifier, TScope, TDocument> : RevisionHandlerBase<TEntity, TIdentifier, TScope?, TDocument> where TEntity : IEntity<TIdentifier>, IRevised where TIdentifier : IEquatable<TIdentifier> where TScope : IDynamoDBContext?
{
    public RevisionHandler(IMapper? mapper) : base(mapper) { }

    public RevisionHandler(RevisionOptions? options, IMapper? mapper) : base(options, mapper) { }

    public override IEnumerable<TEntity> Revise(IEnumerable<TEntity> entities, TScope? scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var table = scope.GetTargetTable<TDocument>();
        var batch = table.CreateBatchWrite();
        var list = entities.ToList();
        var documents = list.Select(MapToData<TDocument>);

        foreach (var document in documents)
            batch.AddDocumentToPut(scope.ToDocument(document));

        Task.Run(async () => await batch.ExecuteAsync());

        return list;
    }

    public override async Task<IEnumerable<TEntity>> Revise(IEnumerable<TEntity> entities, TScope? scope, CancellationToken cancellationToken)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var table = scope.GetTargetTable<TDocument>();
        var batch = table.CreateBatchWrite();
        var list = entities.ToList();
        var documents = list.Select(MapToData<TDocument>);

        foreach (var document in documents)
            batch.AddDocumentToPut(scope.ToDocument(document));

        await batch.ExecuteAsync(cancellationToken);

        return list;
    }

    protected override Boolean DoRevise(TEntity entity, TScope? scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var task = Task.Run(async () => await DoRevise(entity, scope, default));
        var result = task.Result;

        return result;
    }

    protected override async Task<Boolean> DoRevise(TEntity entity, TScope? scope, CancellationToken cancellationToken)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var document = MapToData<TDocument>(entity) ?? throw new NullReferenceException();
        var table = scope.GetTargetTable<TDocument>();

        await table.PutItemAsync(scope.ToDocument(document), cancellationToken);

        return true;
    }
}
