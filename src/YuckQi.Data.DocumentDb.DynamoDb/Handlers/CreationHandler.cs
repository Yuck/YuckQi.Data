using System;
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

    protected override Maybe<TIdentifier?> DoCreate(TEntity entity, TScope scope)
    {
        var task = Task.Run(async () => await DoCreate(entity, scope, default));
        var result = task.Result;

        return result;
    }

    protected override async Task<Maybe<TIdentifier?>> DoCreate(TEntity entity, TScope scope, CancellationToken cancellationToken)
    {
        var document = MapToData<TDocument>(entity);
        if (document == null)
            throw new NullReferenceException();

        var table = scope.GetTargetTable<TDocument>();

        await table.PutItemAsync(scope.ToDocument(document), cancellationToken);

        return Maybe<TIdentifier?>.From(entity.Identifier);
    }
}
