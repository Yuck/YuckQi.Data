using MongoDB.Driver;
using YuckQi.Data.DocumentDb.MongoDb.Extensions;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Handlers.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.DocumentDb.MongoDb.Handlers;

public class RevisionHandler<TEntity, TIdentifier, TScope> : RevisionHandler<TEntity, TIdentifier, TScope?, TEntity> where TEntity : IEntity<TIdentifier>, IRevised where TIdentifier : IEquatable<TIdentifier> where TScope : IClientSessionHandle?
{
    public RevisionHandler() : this(null) { }

    public RevisionHandler(RevisionOptions? options) : base(options, null) { }
}

public class RevisionHandler<TEntity, TIdentifier, TScope, TDocument> : RevisionHandlerBase<TEntity, TIdentifier, TScope?> where TEntity : IEntity<TIdentifier>, IRevised where TIdentifier : IEquatable<TIdentifier> where TScope : IClientSessionHandle?
{
    private static readonly Type DocumentType = typeof(TDocument);

    public RevisionHandler(IMapper? mapper) : base(mapper) { }

    public RevisionHandler(RevisionOptions? options, IMapper? mapper) : base(options, mapper) { }

    public override IEnumerable<TEntity> Revise(IEnumerable<TEntity> entities, TScope? scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var list = entities.ToList();
        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var field = DocumentType.GetIdentifierFieldDefinition<TDocument, TIdentifier>();
        var documents = MapToDataCollection<TDocument>(list) ?? throw new NullReferenceException();

        foreach (var document in documents)
        {
            var identifier = document != null ? document.GetIdentifier<TDocument, TIdentifier>() : default;
            var filter = Builders<TDocument>.Filter.Eq(field, identifier);

            collection.ReplaceOne(scope, filter, document);
        }

        return list;
    }

    public override async Task<IEnumerable<TEntity>> Revise(IEnumerable<TEntity> entities, TScope? scope, CancellationToken cancellationToken)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var list = entities.ToList();
        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var field = DocumentType.GetIdentifierFieldDefinition<TDocument, TIdentifier>();
        var documents = MapToDataCollection<TDocument>(list) ?? throw new NullReferenceException();

        foreach (var document in documents)
        {
            var identifier = document != null ? document.GetIdentifier<TDocument, TIdentifier>() : default;
            var filter = Builders<TDocument>.Filter.Eq(field, identifier);

            await collection.ReplaceOneAsync(scope, filter, document, cancellationToken: cancellationToken);
        }

        return list;
    }

    protected override Boolean DoRevise(TEntity entity, TScope? scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var field = DocumentType.GetIdentifierFieldDefinition<TDocument, TIdentifier>();
        var document = MapToData<TDocument>(entity);
        var identifier = document != null ? document.GetIdentifier<TDocument, TIdentifier>() : default;
        var filter = Builders<TDocument>.Filter.Eq(field, identifier);
        if (document == null)
            throw new NullReferenceException();

        var result = collection.ReplaceOne(scope, filter, document);

        return result.ModifiedCount > 0;
    }

    protected override async Task<Boolean> DoRevise(TEntity entity, TScope? scope, CancellationToken cancellationToken)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var field = DocumentType.GetIdentifierFieldDefinition<TDocument, TIdentifier>();
        var document = MapToData<TDocument>(entity);
        var identifier = document != null ? document.GetIdentifier<TDocument, TIdentifier>() : default;
        var filter = Builders<TDocument>.Filter.Eq(field, identifier);
        if (document == null)
            throw new NullReferenceException();

        var result = await collection.ReplaceOneAsync(scope, filter, document, cancellationToken: cancellationToken);

        return result.ModifiedCount > 0;
    }
}
