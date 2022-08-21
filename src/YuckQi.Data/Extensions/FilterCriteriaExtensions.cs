using YuckQi.Data.Filtering;

namespace YuckQi.Data.Extensions;

public static class FilterCriteriaExtensions
{
    public static IReadOnlyCollection<FilterCriteria> ToFilterCollection(this Object? parameters)
    {
        return parameters switch
        {
            FilterCriteria filter => new List<FilterCriteria> { filter },
            IEnumerable<FilterCriteria> filters => filters.ToList(),
            _ => parameters != null ? parameters.GetType().GetProperties().Select(t => new FilterCriteria(t.Name, FilterOperation.Equal, t.GetValue(parameters))).ToList() : new List<FilterCriteria>()
        };
    }
}
