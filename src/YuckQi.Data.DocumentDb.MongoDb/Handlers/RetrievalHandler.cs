using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using YuckQi.Data.DocumentDb.MongoDb.Extensions;
using YuckQi.Data.Filtering;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.DocumentDb.MongoDb.Handlers;

public class RetrievalHandler<TEntity, TIdentifier, TScope, TDocument> : RetrievalHandlerBase<TEntity, TIdentifier, TScope> where TEntity : IEntity<TIdentifier> where TIdentifier : struct where TScope : IClientSessionHandle
{
    #region Private Members

    private static readonly Type DocumentType = typeof(TDocument);

    #endregion


    #region Constructors

    public RetrievalHandler(IMapper mapper) : base(mapper) { }

    #endregion


    #region Public Methods

    protected override TEntity DoGet(TIdentifier identifier, TScope scope)
    {
        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var field = DocumentType.GetIdentifierFieldDefinition<TDocument, TIdentifier>();
        var filter = Builders<TDocument>.Filter.Eq(field, identifier);
        var reader = collection.FindSync(filter);
        var document = GetDocument(reader);
        var entity = Mapper.Map<TEntity>(document);

        return entity;
    }

    protected override async Task<TEntity> DoGetAsync(TIdentifier identifier, TScope scope)
    {
        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var field = DocumentType.GetIdentifierFieldDefinition<TDocument, TIdentifier>();
        var filter = Builders<TDocument>.Filter.Eq(field, identifier);
        var reader = await collection.FindAsync(filter);
        var document = GetDocument(reader);
        var entity = Mapper.Map<TEntity>(document);

        return entity;
    }

    protected override TEntity DoGet(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
    {
        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var filter = parameters.ToFilterDefinition<TDocument>();
        var reader = collection.FindSync(filter);
        var document = GetDocument(reader);
        var entity = Mapper.Map<TEntity>(document);

        return entity;
    }

    protected override async Task<TEntity> DoGetAsync(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
    {
        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var filter = parameters.ToFilterDefinition<TDocument>();
        var reader = await collection.FindAsync(filter);
        var document = GetDocument(reader);
        var entity = Mapper.Map<TEntity>(document);

        return entity;
    }

    protected override IReadOnlyCollection<TEntity> DoGetList(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
    {
        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var filter = parameters.ToFilterDefinition<TDocument>();
        var reader = collection.FindSync(filter);
        var documents = GetDocuments(reader);
        var entities = Mapper.Map<IReadOnlyCollection<TEntity>>(documents);

        return entities;
    }

    protected override async Task<IReadOnlyCollection<TEntity>> DoGetListAsync(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
    {
        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var filter = parameters.ToFilterDefinition<TDocument>();
        var reader = await collection.FindAsync(filter);
        var documents = GetDocuments(reader);
        var entities = Mapper.Map<IReadOnlyCollection<TEntity>>(documents);

        return entities;
    }

    #endregion


    #region Supporting Methods

    private static TDocument GetDocument(IAsyncCursor<TDocument> reader) => reader.MoveNext() ? reader.Current.SingleOrDefault() : default;

    private static IEnumerable<TDocument> GetDocuments(IAsyncCursor<TDocument> reader)
    {
        var documents = new List<TDocument>();

        while (reader.MoveNext())
            documents.AddRange(reader.Current);

        return documents;
    }

    #endregion
}
