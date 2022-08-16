using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using YuckQi.Data.DocumentDb.MongoDb.Extensions;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Handlers.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.DocumentDb.MongoDb.Handlers;

public class CreationHandler<TEntity, TIdentifier, TScope, TDocument> : CreationHandlerBase<TEntity, TIdentifier, TScope> where TEntity : IEntity<TIdentifier>, ICreated where TIdentifier : struct where TScope : IClientSessionHandle
{
    #region Private Members

    private static readonly Type DocumentType = typeof(TDocument);

    #endregion


    #region Constructors

    public CreationHandler(IMapper mapper) : base(mapper) { }

    public CreationHandler(IMapper mapper, CreationOptions<TIdentifier> options) : base(mapper, options) { }

    #endregion


    #region Protected Methods

    protected override TIdentifier? DoCreate(TEntity entity, TScope scope)
    {
        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var document = Mapper.Map<TDocument>(entity);

        collection.InsertOne(scope, document);

        return document.GetIdentifier<TDocument, TIdentifier>();
    }

    protected override async Task<TIdentifier?> DoCreate(TEntity entity, TScope scope, CancellationToken cancellationToken)
    {
        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var document = Mapper.Map<TDocument>(entity);

        await collection.InsertOneAsync(scope, document, cancellationToken: cancellationToken);

        return document.GetIdentifier<TDocument, TIdentifier>();
    }

    #endregion
}
