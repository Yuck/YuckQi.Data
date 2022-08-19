using System;
using System.Linq;
using System.Linq.Expressions;

namespace YuckQi.Data.Sorting;

public static class SortCriteriaExtensions
{
    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, IOrderedEnumerable<SortCriteria> sort)
    {
        return sort.Aggregate(source.OrderBy(t => 1), (current, item) => item.Order == SortOrder.Ascending ? current.OrderBy(item.Expression) : current.OrderByDescending(item.Expression));
    }

    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, String fieldName) => source.OrderBy(ToLambda<T>(fieldName));

    public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, String fieldName) => source.OrderByDescending(ToLambda<T>(fieldName));

    private static Expression<Func<T, Object>> ToLambda<T>(String fieldName)
    {
        var parameter = Expression.Parameter(typeof(T));
        var property = Expression.Property(parameter, fieldName);
        var valueConverter = Expression.Convert(property, typeof(Object));

        return Expression.Lambda<Func<T, Object>>(valueConverter, parameter);
    }
}
