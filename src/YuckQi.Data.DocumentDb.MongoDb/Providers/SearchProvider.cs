using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using MongoDB.Driver;
using YuckQi.Data.DocumentDb.MongoDb.Extensions;
using YuckQi.Data.DocumentDb.MongoDb.Providers.Abstract;
using YuckQi.Data.Extensions;
using YuckQi.Data.Filtering;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Data.Sorting;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Domain.ValueObjects;
using YuckQi.Domain.ValueObjects.Abstract;

namespace YuckQi.Data.DocumentDb.MongoDb.Providers
{
    public class SearchProvider<TEntity, TKey, TScope, TRecord> : MongoProviderBase<TKey, TRecord>, ISearchProvider<TEntity, TKey, TScope> where TEntity : IEntity<TKey> where TKey : struct where TScope : IClientSessionHandle
    {
        #region Public Methods

        public IPage<TEntity> Search(IReadOnlyCollection<FilterCriteria> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, TScope scope)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (page == null)
                throw new ArgumentNullException(nameof(page));
            if (sort == null)
                throw new ArgumentNullException(nameof(sort));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            var database = scope.Client.GetDatabase(DatabaseName);
            var collection = database.GetCollection<TRecord>(CollectionName);
            var filter = parameters.ToFilterDefinition<TRecord>();
            var records = collection.Find(filter)
                                    .Sort(GetSortDefinition(sort))
                                    .Skip((page.PageNumber - 1) * page.PageSize)
                                    .Limit(page.PageSize)
                                    .ToList();
            var entities = records.Adapt<IReadOnlyCollection<TEntity>>();
            var total = Count(parameters, scope);

            return new Page<TEntity>(entities, (Int32) total, page.PageNumber, page.PageSize);
        }

        public async Task<IPage<TEntity>> SearchAsync(IReadOnlyCollection<FilterCriteria> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, TScope scope)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (page == null)
                throw new ArgumentNullException(nameof(page));
            if (sort == null)
                throw new ArgumentNullException(nameof(sort));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            var database = scope.Client.GetDatabase(DatabaseName);
            var collection = database.GetCollection<TRecord>(CollectionName);
            var filter = parameters.ToFilterDefinition<TRecord>();
            var records = collection.Find(filter)
                                    .Sort(GetSortDefinition(sort))
                                    .Skip((page.PageNumber - 1) * page.PageSize)
                                    .Limit(page.PageSize)
                                    .ToListAsync();
            var entities = records.Adapt<IReadOnlyCollection<TEntity>>();
            var total = await CountAsync(parameters, scope);

            return new Page<TEntity>(entities, (Int32) total, page.PageNumber, page.PageSize);
        }

        public IPage<TEntity> Search(Object parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, TScope scope) => Search(parameters?.ToFilterCollection(), page, sort, scope);

        public Task<IPage<TEntity>> SearchAsync(Object parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, TScope scope) => SearchAsync(parameters?.ToFilterCollection(), page, sort, scope);

        #endregion


        #region Supporting Methods

        private static Int64 Count(IEnumerable<FilterCriteria> parameters, TScope scope)
        {
            var database = scope.Client.GetDatabase(DatabaseName);
            var collection = database.GetCollection<TRecord>(CollectionName);
            var filter = parameters.ToFilterDefinition<TRecord>();
            var total = collection.CountDocuments(filter);

            return total;
        }

        private static Task<Int64> CountAsync(IEnumerable<FilterCriteria> parameters, TScope scope)
        {
            var database = scope.Client.GetDatabase(DatabaseName);
            var collection = database.GetCollection<TRecord>(CollectionName);
            var filter = parameters.ToFilterDefinition<TRecord>();
            var total = collection.CountDocumentsAsync(filter);

            return total;
        }

        private static SortDefinition<TRecord> GetSortDefinition(IEnumerable<SortCriteria> criteria)
        {
            var sorts = criteria.Select(t =>
            {
                var field = new StringFieldDefinition<TRecord>(t.Expression);
                var sort = t.Order == SortOrder.Ascending ? Builders<TRecord>.Sort.Ascending(field) : Builders<TRecord>.Sort.Descending(field);

                return sort;
            });
            var combined = Builders<TRecord>.Sort.Combine(sorts);

            return combined;
        }

        #endregion
    }
}
