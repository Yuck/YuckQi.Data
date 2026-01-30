using YuckQi.Data.Filtering;
using YuckQi.Data.Sorting;
using YuckQi.Domain.ValueObjects.Abstract;

namespace YuckQi.Data.Sql.Dapper.Abstract.Interfaces;

public interface ISqlGenerator
{
    string GenerateCountQuery(IReadOnlyCollection<FilterCriteria> parameters);

    string GenerateGetQuery(IReadOnlyCollection<FilterCriteria>? parameters);

    string GenerateSearchQuery(IReadOnlyCollection<FilterCriteria> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort);
}
