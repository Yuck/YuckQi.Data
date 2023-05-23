using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.DocumentDb.DynamoDb.Handlers;

public class PhysicalDeletionHandler<TEntity, TIdentifier, TScope, TDocument> : PhysicalDeletionHandlerBase<TEntity, TIdentifier, TScope> where TEntity : IEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier> where TScope : IDynamoDBContext
{
    public PhysicalDeletionHandler(IMapper mapper) : base(mapper) { }

    protected override Boolean DoDelete(TEntity entity, TScope scope)
    {
        var task = Task.Run(async () => await DoDelete(entity, scope, default));
        var result = task.Result;

        return result;
    }

    protected override async Task<Boolean> DoDelete(TEntity entity, TScope scope, CancellationToken cancellationToken)
    {
        var document = MapToData<TDocument>(entity);
        if (document == null)
            throw new NullReferenceException();

        var table = scope.GetTargetTable<TDocument>();

        await table.DeleteItemAsync(scope.ToDocument(document), cancellationToken);

        return true;
    }
}
