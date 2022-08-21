using MongoDB.Driver;
using YuckQi.Data.DocumentDb.MongoDb.Extensions;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Handlers.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.DocumentDb.MongoDb.Handlers;

public class RevisionHandler<TEntity, TIdentifier, TScope> : RevisionHandler<TEntity, TIdentifier, TScope, TEntity> where TEntity : IEntity<TIdentifier>, IRevised where TIdentifier : struct where TScope : IClientSessionHandle
{
    #region Constructors

    public RevisionHandler() : this(null) { }

    public RevisionHandler(RevisionOptions? options) : base(options, null) { }

    #endregion
}

public class RevisionHandler<TEntity, TIdentifier, TScope, TDocument> : RevisionHandlerBase<TEntity, TIdentifier, TScope> where TEntity : IEntity<TIdentifier>, IRevised where TIdentifier : struct where TScope : IClientSessionHandle
{
    #region Constructors

    public RevisionHandler(IMapper? mapper) : base(mapper) { }

    public RevisionHandler(RevisionOptions? options, IMapper? mapper) : base(options, mapper) { }

    #endregion


    #region Private Members

    private static readonly Type DocumentType = typeof(TDocument);

    #endregion


    #region Protected Methods

    protected override Boolean DoRevise(TEntity entity, TScope scope)
    {
        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var field = DocumentType.GetIdentifierFieldDefinition<TDocument, TIdentifier>();
        var document = MapToData<TDocument>(entity);
        var identifier = document?.GetIdentifier<TDocument, TIdentifier>();
        var filter = Builders<TDocument>.Filter.Eq(field, identifier);
        if (document == null)
            throw new NullReferenceException();

        var result = collection.ReplaceOne(scope, filter, document);

        return result.ModifiedCount > 0;
    }

    protected override async Task<Boolean> DoRevise(TEntity entity, TScope scope, CancellationToken cancellationToken)
    {
        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var field = DocumentType.GetIdentifierFieldDefinition<TDocument, TIdentifier>();
        var document = MapToData<TDocument>(entity);
        var identifier = document?.GetIdentifier<TDocument, TIdentifier>();
        var filter = Builders<TDocument>.Filter.Eq(field, identifier);
        if (document == null)
            throw new NullReferenceException();

        var result = await collection.ReplaceOneAsync(scope, filter, document, cancellationToken: cancellationToken);

        return result.ModifiedCount > 0;
    }

    #endregion
}
