using System;

namespace YuckQi.Data.Sorting;

public readonly struct SortCriteria
{
    public String Expression { get; }
    public SortOrder Order { get; }

    public SortCriteria(String expression, SortOrder order)
    {
        Expression = expression ?? throw new ArgumentNullException(nameof(expression));
        Order = order;
    }
}
