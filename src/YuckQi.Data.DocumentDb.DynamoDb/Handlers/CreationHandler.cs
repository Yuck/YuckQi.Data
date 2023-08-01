using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using CSharpFunctionalExtensions;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Handlers.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.DocumentDb.DynamoDb.Handlers;

public class CreationHandler<TEntity, TIdentifier, TScope, TDocument> : CreationHandlerBase<TEntity, TIdentifier, TScope> where TEntity : IEntity<TIdentifier>, ICreated where TIdentifier : IEquatable<TIdentifier> where TScope : IDynamoDBContext
{
    public CreationHandler(IMapper mapper) : base(mapper) { }

    public CreationHandler(CreationOptions<TIdentifier> options, IMapper mapper) : base(options, mapper) { }

    public override IEnumerable<TEntity> Create(IEnumerable<TEntity> entities, TScope scope)
    {
        var table = scope.GetTargetTable<TDocument>();
        var batch = table.CreateBatchWrite();
        var list = entities.ToList();
        var documents = list.Select(MapToData<TDocument>);

        foreach (var document in documents)
            batch.AddDocumentToPut(scope.ToDocument(document));

        Task.Run(async () => await batch.ExecuteAsync());

        return list;
    }

    public override async Task<IEnumerable<TEntity>> Create(IEnumerable<TEntity> entities, TScope scope, CancellationToken cancellationToken)
    {
        var table = scope.GetTargetTable<TDocument>();
        var batch = table.CreateBatchWrite();
        var list = entities.ToList();
        var documents = list.Select(MapToData<TDocument>);

        foreach (var document in documents)
            batch.AddDocumentToPut(scope.ToDocument(document));

        await batch.ExecuteAsync(cancellationToken);

        return list;
    }

    protected override Maybe<TIdentifier?> DoCreate(TEntity entity, TScope scope)
    {
        var task = Task.Run(async () => await DoCreate(entity, scope, default));
        var result = task.Result;

        return result;
    }

    protected override async Task<Maybe<TIdentifier?>> DoCreate(TEntity entity, TScope scope, CancellationToken cancellationToken)
    {
        var document = MapToData<TDocument>(entity) ?? throw new NullReferenceException();
        var table = scope.GetTargetTable<TDocument>();

        await table.PutItemAsync(scope.ToDocument(document), cancellationToken);

        return Maybe<TIdentifier?>.From(entity.Identifier);
    }
}
