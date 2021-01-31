using System;

namespace YuckQi.Data.Sorting
{
    public readonly struct SortCriteria
    {
        public string Expression { get; }
        public SortOrder Order { get; }

        public SortCriteria(string expression, SortOrder order)
        {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
            Order = order;
        }
    }
}