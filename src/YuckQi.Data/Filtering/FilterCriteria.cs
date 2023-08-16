using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace YuckQi.Data.Filtering;

public readonly struct FilterCriteria
{
    public String FieldName { get; }
    public FilterOperation Operation { get; }
    public Object? Value { get; }

    public FilterCriteria(String fieldName, Object? value) : this(fieldName, FilterOperation.Equal, value) { }

    public FilterCriteria(String fieldName, FilterOperation operation, Object? value)
    {
        FieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
        Operation = operation;
        Value = value;
    }

    public Func<Boolean> ToExpression<T>(T instance)
    {
        var property = Expression.Property(Expression.Constant(instance), FieldName);
        var value = Expression.Constant(Value);
        var body = Operation switch
        {
            FilterOperation.Equal => Expression.Equal(property, value) as Expression,
            FilterOperation.GreaterThan => Expression.GreaterThan(property, value),
            FilterOperation.GreaterThanOrEqual => Expression.GreaterThanOrEqual(property, value),
            FilterOperation.In => Expression.Call(GetContainsMethodInfo(property), Expression.Constant(Value as IEnumerable), property),
            FilterOperation.LessThan => Expression.LessThan(property, value),
            FilterOperation.LessThanOrEqual => Expression.LessThanOrEqual(property, value),
            FilterOperation.NotEqual => Expression.NotEqual(property, value),
            _ => throw new NotSupportedException()
        };

        return Expression.Lambda<Func<Boolean>>(body).Compile();
    }

    private static MethodInfo GetContainsMethodInfo(Expression property)
    {
        var methods = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public);
        var contains = methods.Single(t => t.Name == "Contains" && t.GetParameters().Length == 2);
        var generic = contains.MakeGenericMethod(property.Type);

        return generic;
    }
}
