using System;
using System.Collections.Generic;
using System.Linq;
using YuckQi.Data.Filtering;

namespace YuckQi.Data.Extensions;

public static class FilterCriteriaExtensions
{
    public static IReadOnlyCollection<FilterCriteria> ToFilterCollection(this Object parameters)
    {
        switch (parameters)
        {
            case FilterCriteria filter:
                return new List<FilterCriteria> { filter };
            case IEnumerable<FilterCriteria> filters:
                return filters.ToList();

            default:
                return parameters != null
                           ? parameters.GetType().GetProperties().Select(t => new FilterCriteria(t.Name, FilterOperation.Equal, t.GetValue(parameters))).ToList()
                           : new List<FilterCriteria>();
        }
    }
}
