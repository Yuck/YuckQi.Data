using System;

namespace YuckQi.Data.Sorting
{
    public readonly struct SortCriteria<T> where T : class
    {
        public T Expression { get; }
        public SortOrder Order { get; }

        public SortCriteria(T expression, SortOrder order)
        {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
            Order = order;
        }
    }
}