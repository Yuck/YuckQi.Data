using YuckQi.Data.Filtering;
using YuckQi.Data.Sorting;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Domain.ValueObjects.Abstract;

namespace YuckQi.Data.Handlers.Abstract;

public interface ISearchHandler<TEntity, TIdentifier, in TScope> where TEntity : IEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier>
{
    IPage<TEntity> Search(IReadOnlyCollection<FilterCriteria> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, TScope? scope);

    Task<IPage<TEntity>> Search(IReadOnlyCollection<FilterCriteria> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, TScope? scope, CancellationToken cancellationToken);

    IPage<TEntity> Search(Object parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, TScope? scope);

    Task<IPage<TEntity>> Search(Object parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, TScope? scope, CancellationToken cancellationToken);
}
