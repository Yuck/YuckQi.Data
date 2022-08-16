using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YuckQi.Data.Extensions;
using YuckQi.Data.Filtering;
using YuckQi.Data.Sorting;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Domain.ValueObjects;
using YuckQi.Domain.ValueObjects.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Handlers.Abstract;

public abstract class SearchHandlerBase<TEntity, TIdentifier, TScope> : ISearchHandler<TEntity, TIdentifier, TScope> where TEntity : IEntity<TIdentifier> where TIdentifier : struct
{
    #region Properties

    protected IMapper Mapper { get; }

    #endregion


    #region Constructors

    protected SearchHandlerBase(IMapper mapper)
    {
        Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    #endregion


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

        var entities = DoSearch(parameters, page, sort, scope);
        var total = DoCount(parameters, scope);

        return new Page<TEntity>(entities, total, page.PageNumber, page.PageSize);
    }

    public async Task<IPage<TEntity>> Search(IReadOnlyCollection<FilterCriteria> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, TScope scope, CancellationToken cancellationToken)
    {
        if (parameters == null)
            throw new ArgumentNullException(nameof(parameters));
        if (page == null)
            throw new ArgumentNullException(nameof(page));
        if (sort == null)
            throw new ArgumentNullException(nameof(sort));
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        var entities = await DoSearch(parameters, page, sort, scope, cancellationToken);
        var total = await DoCount(parameters, scope, cancellationToken);

        return new Page<TEntity>(entities, total, page.PageNumber, page.PageSize);
    }

    public IPage<TEntity> Search(Object parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, TScope scope) => Search(parameters?.ToFilterCollection(), page, sort, scope);

    public Task<IPage<TEntity>> Search(Object parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, TScope scope, CancellationToken cancellationToken) => Search(parameters?.ToFilterCollection(), page, sort, scope, cancellationToken);

    #endregion


    #region Protected Methods

    protected abstract Int32 DoCount(IReadOnlyCollection<FilterCriteria> parameters, TScope scope);

    protected abstract Task<Int32> DoCount(IReadOnlyCollection<FilterCriteria> parameters, TScope scope, CancellationToken cancellationToken);

    protected abstract IReadOnlyCollection<TEntity> DoSearch(IReadOnlyCollection<FilterCriteria> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, TScope scope);

    protected abstract Task<IReadOnlyCollection<TEntity>> DoSearch(IReadOnlyCollection<FilterCriteria> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, TScope scope, CancellationToken cancellationToken);

    #endregion
}
