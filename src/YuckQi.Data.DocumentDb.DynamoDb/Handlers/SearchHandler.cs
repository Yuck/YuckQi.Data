using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using YuckQi.Data.DocumentDb.DynamoDb.Extensions;
using YuckQi.Data.Filtering;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Sorting;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Domain.ValueObjects.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.DocumentDb.DynamoDb.Handlers;

public class SearchHandler<TEntity, TIdentifier, TScope, TDocument> : SearchHandlerBase<TEntity, TIdentifier, TScope> where TEntity : IEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier> where TScope : IDynamoDBContext
{
    public SearchHandler(IMapper mapper) : base(mapper) { }

    protected override Int32 DoCount(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
    {
        var task = Task.Run(async () => await DoCount(parameters, scope, default));
        var result = task.Result;

        return result;
    }

    protected override Task<Int32> DoCount(IReadOnlyCollection<FilterCriteria> parameters, TScope scope, CancellationToken cancellationToken)
    {
        var table = scope.GetTargetTable<TDocument>();
        var filter = parameters.ToScanFilter();
        var search = table.Scan(filter);
        var count = search.Count;

        return Task.FromResult(count);
    }

    protected override IReadOnlyCollection<TEntity> DoSearch(IReadOnlyCollection<FilterCriteria> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, TScope scope)
    {
        var task = Task.Run(async () => await DoSearch(parameters, page, sort, scope, default));
        var result = task.Result;

        return result;
    }

    protected override async Task<IReadOnlyCollection<TEntity>> DoSearch(IReadOnlyCollection<FilterCriteria> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, TScope scope, CancellationToken cancellationToken)
    {
        if (sort.Count() > 1)
            throw new ApplicationException("no, you can't; ddb only supports sorting by a single attribute at a time"); // TODO: Probably not AppException; shouldn't use this anywhere in library code

        var table = scope.GetTargetTable<TDocument>();
        var filter = parameters.ToScanFilter();
        var configuration = new ScanOperationConfig
        {
            Filter = filter,
            Limit = page.PageSize
        };

        if (sort.Any())
        {
            var s = sort.SingleOrDefault();

            configuration.IndexName = s.Expression;
        }

        // TODO: How to start at a specific page of data?

        var search = table.Scan(configuration);
        var results = await search.GetRemainingAsync(cancellationToken); // TODO: Need supporting methods like for MongoDB where we read a set at a time then get remaining as the last operation
        var documents = scope.FromDocuments<TDocument>(results);
        var entities = MapToEntityCollection(documents);

        return entities;
    }
}
