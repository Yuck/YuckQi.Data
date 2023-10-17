using CSharpFunctionalExtensions;
using MongoDB.Driver;
using YuckQi.Data.DocumentDb.MongoDb.Extensions;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Handlers.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.DocumentDb.MongoDb.Handlers;

public class CreationHandler<TEntity, TIdentifier, TScope> : CreationHandler<TEntity, TIdentifier, TScope?, TEntity> where TEntity : IEntity<TIdentifier>, ICreated where TIdentifier : struct, IEquatable<TIdentifier> where TScope : IClientSessionHandle?
{
    public CreationHandler() : this(null) { }

    public CreationHandler(CreationOptions<TIdentifier>? options) : base(options, null) { }
}

public class CreationHandler<TEntity, TIdentifier, TScope, TDocument> : CreationHandlerBase<TEntity, TIdentifier, TScope?> where TEntity : IEntity<TIdentifier>, ICreated where TIdentifier : struct, IEquatable<TIdentifier> where TScope : IClientSessionHandle?
{
    private static readonly Type DocumentType = typeof(TDocument);

    public CreationHandler(IMapper? mapper) : base(mapper) { }

    public CreationHandler(CreationOptions<TIdentifier>? options, IMapper? mapper) : base(options, mapper) { }

    public override IEnumerable<TEntity> Create(IEnumerable<TEntity> entities, TScope? scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var list = entities.ToList();
        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var documents = MapToDataCollection<TDocument>(list) ?? throw new NullReferenceException();

        collection.InsertMany(scope, documents);

        return list;
    }

    public override async Task<IEnumerable<TEntity>> Create(IEnumerable<TEntity> entities, TScope? scope, CancellationToken cancellationToken)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var list = entities.ToList();
        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var documents = MapToDataCollection<TDocument>(list) ?? throw new NullReferenceException();

        await collection.InsertManyAsync(scope, documents, cancellationToken: cancellationToken);

        return list;
    }

    protected override Maybe<TIdentifier> DoCreate(TEntity entity, TScope? scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var document = MapToData<TDocument>(entity) ?? throw new NullReferenceException();

        collection.InsertOne(scope, document);

        return document.GetIdentifier<TDocument, TIdentifier>();
    }

    protected override async Task<Maybe<TIdentifier>> DoCreate(TEntity entity, TScope? scope, CancellationToken cancellationToken)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var document = MapToData<TDocument>(entity) ?? throw new NullReferenceException();

        await collection.InsertOneAsync(scope, document, cancellationToken: cancellationToken);

        return document.GetIdentifier<TDocument, TIdentifier>();
    }
}
