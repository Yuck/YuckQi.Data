using System;
using System.Linq.Expressions;

namespace YuckQi.Data.Filtering;

public readonly struct FilterCriteria
{
    public String FieldName { get; }
    public FilterOperation Operation { get; }
    public Object Value { get; }

    public FilterCriteria(String fieldName, Object value) : this(fieldName, FilterOperation.Equal, value) { }

    public FilterCriteria(String fieldName, FilterOperation operation, Object value)
    {
        FieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
        Operation = operation;
        Value = value;
    }

    public Func<Boolean> ToExpression<T>(T instance)
    {
        var property = Expression.Property(Expression.Constant(instance), FieldName);
        var value = Expression.Constant(Value);

        return Operation switch
        {
            FilterOperation.Equal => Expression.Lambda<Func<Boolean>>(Expression.Equal(property, value)).Compile(),
            FilterOperation.GreaterThan => Expression.Lambda<Func<Boolean>>(Expression.GreaterThan(property, value)).Compile(),
            FilterOperation.GreaterThanOrEqual => Expression.Lambda<Func<Boolean>>(Expression.GreaterThanOrEqual(property, value)).Compile(),
            FilterOperation.LessThan => Expression.Lambda<Func<Boolean>>(Expression.LessThan(property, value)).Compile(),
            FilterOperation.LessThanOrEqual => Expression.Lambda<Func<Boolean>>(Expression.LessThanOrEqual(property, value)).Compile(),
            FilterOperation.NotEqual => Expression.Lambda<Func<Boolean>>(Expression.NotEqual(property, value)).Compile(),
            _ => throw new NotSupportedException()
        };
    }
}
