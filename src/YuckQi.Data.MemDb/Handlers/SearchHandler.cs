using System.Collections.Concurrent;
using YuckQi.Data.Filtering;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Sorting;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Domain.ValueObjects.Abstract;

namespace YuckQi.Data.MemDb.Handlers;

public class SearchHandler<TEntity, TIdentifier, TScope> : SearchHandlerBase<TEntity, TIdentifier, TScope> where TEntity : IEntity<TIdentifier> where TIdentifier : struct
{
    private readonly ConcurrentDictionary<TIdentifier, TEntity> _entities;

    public SearchHandler(ConcurrentDictionary<TIdentifier, TEntity> entities)
    {
        _entities = entities ?? throw new ArgumentNullException(nameof(entities));
    }

    protected override Int32 DoCount(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
    {
        var entities = GetEntities(parameters);
        var count = entities.Count();

        return count;
    }

    protected override Task<Int32> DoCount(IReadOnlyCollection<FilterCriteria> parameters, TScope scope, CancellationToken cancellationToken) => Task.FromResult(DoCount(parameters, scope));

    protected override IReadOnlyCollection<TEntity> DoSearch(IReadOnlyCollection<FilterCriteria> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, TScope scope)
    {
        var entities = GetEntities(parameters);
        var sorted = entities; // TODO: Apply "sort"       
        var paged = sorted.Skip((page.PageNumber - 1) * page.PageSize).Take(page.PageSize);

        return paged.ToList();
    }

    protected override Task<IReadOnlyCollection<TEntity>> DoSearch(IReadOnlyCollection<FilterCriteria> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, TScope scope, CancellationToken cancellationToken) => Task.FromResult(DoSearch(parameters, page, sort, scope));

    private IEnumerable<TEntity> GetEntities(IReadOnlyCollection<FilterCriteria> parameters)
    {
        return _entities.Values.Where(entity => parameters.Select(t => t.ToExpression(entity)).All(t => t()));
    }
}
