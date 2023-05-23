using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Handlers.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.DocumentDb.DynamoDb.Handlers;

public class RevisionHandler<TEntity, TIdentifier, TScope, TDocument> : RevisionHandlerBase<TEntity, TIdentifier, TScope> where TEntity : IEntity<TIdentifier>, IRevised where TIdentifier : IEquatable<TIdentifier> where TScope : IDynamoDBContext
{
    public RevisionHandler(IMapper mapper) : base(mapper) { }

    public RevisionHandler(RevisionOptions options, IMapper mapper) : base(options, mapper) { }

    protected override Boolean DoRevise(TEntity entity, TScope scope)
    {
        var task = Task.Run(async () => await DoRevise(entity, scope, default));
        var result = task.Result;

        return result;
    }

    protected override async Task<Boolean> DoRevise(TEntity entity, TScope scope, CancellationToken cancellationToken)
    {
        var document = MapToData<TDocument>(entity);
        if (document == null)
            throw new NullReferenceException();

        var table = scope.GetTargetTable<TDocument>();

        await table.PutItemAsync(scope.ToDocument(document), cancellationToken);

        return true;
    }
}
