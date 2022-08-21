using MongoDB.Driver;
using YuckQi.Data.DocumentDb.MongoDb.Extensions;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Handlers.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.DocumentDb.MongoDb.Handlers;

public class CreationHandler<TEntity, TIdentifier, TScope> : CreationHandler<TEntity, TIdentifier, TScope, TEntity> where TEntity : IEntity<TIdentifier>, ICreated where TIdentifier : struct where TScope : IClientSessionHandle
{
    #region Constructors

    public CreationHandler() : this(null) { }

    public CreationHandler(CreationOptions<TIdentifier>? options) : base(options, null) { }

    #endregion
}

public class CreationHandler<TEntity, TIdentifier, TScope, TDocument> : CreationHandlerBase<TEntity, TIdentifier, TScope> where TEntity : IEntity<TIdentifier>, ICreated where TIdentifier : struct where TScope : IClientSessionHandle
{
    #region Private Members

    private static readonly Type DocumentType = typeof(TDocument);

    #endregion


    #region Constructors

    public CreationHandler(IMapper? mapper) : base(mapper) { }

    public CreationHandler(CreationOptions<TIdentifier>? options, IMapper? mapper) : base(options, mapper) { }

    #endregion


    #region Protected Methods

    protected override TIdentifier? DoCreate(TEntity entity, TScope scope)
    {
        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var document = MapToData<TDocument>(entity);
        if (document == null)
            throw new NullReferenceException();

        collection.InsertOne(scope, document);

        return document.GetIdentifier<TDocument, TIdentifier>();
    }

    protected override async Task<TIdentifier?> DoCreate(TEntity entity, TScope scope, CancellationToken cancellationToken)
    {
        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var document = MapToData<TDocument>(entity);
        if (document == null)
            throw new NullReferenceException();

        await collection.InsertOneAsync(scope, document, cancellationToken: cancellationToken);

        return document.GetIdentifier<TDocument, TIdentifier>();
    }

    #endregion
}
