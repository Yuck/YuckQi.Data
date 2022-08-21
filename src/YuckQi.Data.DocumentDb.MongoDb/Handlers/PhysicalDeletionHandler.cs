using MongoDB.Driver;
using YuckQi.Data.DocumentDb.MongoDb.Extensions;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.DocumentDb.MongoDb.Handlers;

public class PhysicalDeletionHandler<TEntity, TIdentifier, TScope> : PhysicalDeletionHandler<TEntity, TIdentifier, TScope, TEntity> where TEntity : IEntity<TIdentifier> where TIdentifier : struct where TScope : IClientSessionHandle
{
    #region Constructors

    public PhysicalDeletionHandler() : base(null) { }

    #endregion
}

public class PhysicalDeletionHandler<TEntity, TIdentifier, TScope, TDocument> : PhysicalDeletionHandlerBase<TEntity, TIdentifier, TScope> where TEntity : IEntity<TIdentifier> where TIdentifier : struct where TScope : IClientSessionHandle
{
    #region Private Members

    private static readonly Type DocumentType = typeof(TDocument);

    #endregion


    #region Constructors

    public PhysicalDeletionHandler(IMapper? mapper) : base(mapper) { }

    #endregion


    #region Protected Methods

    protected override Boolean DoDelete(TEntity entity, TScope scope)
    {
        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var document = GetDocument(entity);
        var field = DocumentType.GetIdentifierFieldDefinition<TDocument, TIdentifier>();
        var identifier = document?.GetIdentifier<TDocument, TIdentifier>();
        var filter = Builders<TDocument>.Filter.Eq(field, identifier);
        var result = collection.DeleteOne(scope, filter);

        return result.DeletedCount > 0;
    }

    protected override async Task<Boolean> DoDelete(TEntity entity, TScope scope, CancellationToken cancellationToken)
    {
        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var document = GetDocument(entity);
        var field = DocumentType.GetIdentifierFieldDefinition<TDocument, TIdentifier>();
        var identifier = document?.GetIdentifier<TDocument, TIdentifier>();
        var filter = Builders<TDocument>.Filter.Eq(field, identifier);
        var result = await collection.DeleteOneAsync(scope, filter, cancellationToken: cancellationToken);

        return result.DeletedCount > 0;
    }

    #endregion


    #region Supporting Methods

    private TDocument? GetDocument(TEntity entity)
    {
        if (entity is TDocument document)
            return document;

        return Mapper != null ? Mapper.Map<TDocument>(entity) : default;
    }

    #endregion
}
