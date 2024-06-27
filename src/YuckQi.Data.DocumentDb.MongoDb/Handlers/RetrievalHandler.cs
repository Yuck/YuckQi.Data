using MongoDB.Driver;
using YuckQi.Data.DocumentDb.MongoDb.Extensions;
using YuckQi.Data.Filtering;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.DocumentDb.MongoDb.Handlers;

public class RetrievalHandler<TEntity, TIdentifier, TScope> : RetrievalHandler<TEntity, TIdentifier, TScope?, TEntity> where TEntity : IEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier> where TScope : IClientSessionHandle?
{
    public RetrievalHandler() : base(null) { }
}

public class RetrievalHandler<TEntity, TIdentifier, TScope, TDocument> : RetrievalHandlerBase<TEntity, TIdentifier, TScope?> where TEntity : IEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier> where TScope : IClientSessionHandle?
{
    private static readonly Type DocumentType = typeof(TDocument);

    public RetrievalHandler(IMapper? mapper) : base(mapper) { }

    protected override TEntity? DoGet(TIdentifier identifier, TScope? scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var field = DocumentType.GetIdentifierFieldDefinition<TDocument, TIdentifier>();
        var filter = Builders<TDocument>.Filter.Eq(field, identifier);
        var reader = collection.FindSync(filter);
        var document = GetDocument(reader);
        var entity = MapToEntity(document);

        return entity;
    }

    protected override async Task<TEntity?> DoGet(TIdentifier identifier, TScope? scope, CancellationToken cancellationToken)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var field = DocumentType.GetIdentifierFieldDefinition<TDocument, TIdentifier>();
        var filter = Builders<TDocument>.Filter.Eq(field, identifier);
        var reader = await collection.FindAsync(filter, cancellationToken: cancellationToken);
        var document = GetDocument(reader);
        var entity = MapToEntity(document);

        return entity;
    }

    protected override TEntity? DoGet(IReadOnlyCollection<FilterCriteria> parameters, TScope? scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var filter = parameters.ToFilterDefinition<TDocument>();
        var reader = collection.FindSync(filter);
        var document = GetDocument(reader);
        var entity = MapToEntity(document);

        return entity;
    }

    protected override async Task<TEntity?> DoGet(IReadOnlyCollection<FilterCriteria> parameters, TScope? scope, CancellationToken cancellationToken)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var filter = parameters.ToFilterDefinition<TDocument>();
        var reader = await collection.FindAsync(filter, cancellationToken: cancellationToken);
        var document = GetDocument(reader);
        var entity = MapToEntity(document);

        return entity;
    }

    protected override IReadOnlyCollection<TEntity> DoGetList(IReadOnlyCollection<FilterCriteria>? parameters, TScope? scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var filter = parameters.ToFilterDefinition<TDocument>();
        var reader = collection.FindSync(filter);
        var documents = GetDocuments(reader);
        var entities = MapToEntityCollection(documents);

        return entities;
    }

    protected override async Task<IReadOnlyCollection<TEntity>> DoGetList(IReadOnlyCollection<FilterCriteria>? parameters, TScope? scope, CancellationToken cancellationToken)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var filter = parameters.ToFilterDefinition<TDocument>();
        var reader = await collection.FindAsync(filter, cancellationToken: cancellationToken);
        var documents = GetDocuments(reader);
        var entities = MapToEntityCollection(documents);

        return entities;
    }

    private static TDocument? GetDocument(IAsyncCursor<TDocument> reader) => reader.MoveNext() ? reader.Current.SingleOrDefault() : default;

    private static IEnumerable<TDocument> GetDocuments(IAsyncCursor<TDocument> reader)
    {
        var documents = new List<TDocument>();

        while (reader.MoveNext())
            documents.AddRange(reader.Current);

        return documents;
    }
}
