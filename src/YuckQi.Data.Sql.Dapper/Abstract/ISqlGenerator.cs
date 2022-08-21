using YuckQi.Data.Filtering;
using YuckQi.Data.Sorting;
using YuckQi.Domain.ValueObjects.Abstract;

namespace YuckQi.Data.Sql.Dapper.Abstract;

public interface ISqlGenerator<TRecord> // TODO: Do I need TRecord? Check projects that use this...there should only be a single generator registered and I don't think implementations need the constraint?
{
    String GenerateCountQuery(IReadOnlyCollection<FilterCriteria> parameters);

    String GenerateGetQuery(IReadOnlyCollection<FilterCriteria>? parameters);

    String GenerateSearchQuery(IReadOnlyCollection<FilterCriteria> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort);
}
