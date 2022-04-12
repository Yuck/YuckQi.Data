using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using YuckQi.Data.DocumentDb.MongoDb.Extensions;
using YuckQi.Data.Filtering;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Sorting;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Domain.ValueObjects.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.DocumentDb.MongoDb.Handlers;

public class SearchHandler<TEntity, TKey, TScope, TDocument> : SearchHandlerBase<TEntity, TKey, TScope> where TEntity : IEntity<TKey> where TKey : struct where TScope : IClientSessionHandle
{
    #region Private Members

    private static readonly Type DocumentType = typeof(TDocument);

    #endregion


    #region Constructors

    public SearchHandler(IMapper mapper) : base(mapper) { }

    #endregion


    #region Protected Methods

    protected override Int32 DoCount(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
    {
        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var filter = parameters.ToFilterDefinition<TDocument>();
        var total = collection.CountDocuments(filter);

        return (Int32) total;
    }

    protected override async Task<Int32> DoCountAsync(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
    {
        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var filter = parameters.ToFilterDefinition<TDocument>();
        var total = await collection.CountDocumentsAsync(filter);

        return (Int32) total;
    }

    protected override IReadOnlyCollection<TEntity> DoSearch(IReadOnlyCollection<FilterCriteria> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, TScope scope)
    {
        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var filter = parameters.ToFilterDefinition<TDocument>();
        var documents = collection.Find(filter)
                                  .Sort(GetSortDefinition(sort))
                                  .Skip((page.PageNumber - 1) * page.PageSize)
                                  .Limit(page.PageSize)
                                  .ToList();
        var entities = Mapper.Map<IReadOnlyCollection<TEntity>>(documents);

        return entities;
    }

    protected override async Task<IReadOnlyCollection<TEntity>> DoSearchAsync(IReadOnlyCollection<FilterCriteria> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, TScope scope)
    {
        var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
        var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
        var filter = parameters.ToFilterDefinition<TDocument>();
        var documents = await collection.Find(filter)
                                        .Sort(GetSortDefinition(sort))
                                        .Skip((page.PageNumber - 1) * page.PageSize)
                                        .Limit(page.PageSize)
                                        .ToListAsync();
        var entities = Mapper.Map<IReadOnlyCollection<TEntity>>(documents);

        return entities;
    }

    #endregion


    #region Supporting Methods

    private static SortDefinition<TDocument> GetSortDefinition(IEnumerable<SortCriteria> criteria)
    {
        var sorts = criteria.Select(t =>
        {
            var field = new StringFieldDefinition<TDocument>(t.Expression);
            var sort = t.Order == SortOrder.Ascending ? Builders<TDocument>.Sort.Ascending(field) : Builders<TDocument>.Sort.Descending(field);

            return sort;
        });
        var combined = Builders<TDocument>.Sort.Combine(sorts);

        return combined;
    }

    #endregion
}
