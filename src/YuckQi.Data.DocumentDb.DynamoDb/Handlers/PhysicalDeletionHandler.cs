using Amazon.DynamoDBv2.DataModel;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.DocumentDb.DynamoDb.Handlers;

public class PhysicalDeletionHandler<TEntity, TIdentifier, TScope> : PhysicalDeletionHandler<TEntity, TIdentifier, TScope?, TEntity> where TEntity : IEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier> where TScope : IDynamoDBContext?
{
    public PhysicalDeletionHandler() : base(null) { }
}

public class PhysicalDeletionHandler<TEntity, TIdentifier, TScope, TDocument> : PhysicalDeletionHandlerBase<TEntity, TIdentifier, TScope?, TDocument> where TEntity : IEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier> where TScope : IDynamoDBContext?
{
    public PhysicalDeletionHandler(IMapper? mapper) : base(mapper) { }

    protected override Boolean DoDelete(TEntity entity, TScope? scope)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var task = Task.Run(async () => await DoDelete(entity, scope, default));
        var result = task.Result;

        return result;
    }

    protected override async Task<Boolean> DoDelete(TEntity entity, TScope? scope, CancellationToken cancellationToken)
    {
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var document = MapToData<TDocument>(entity) ?? throw new NullReferenceException();
        var table = scope.GetTargetTable<TDocument>();

        await table.DeleteItemAsync(scope.ToDocument(document), cancellationToken);

        return true;
    }
}
