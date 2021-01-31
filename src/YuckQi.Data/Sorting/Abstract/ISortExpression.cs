namespace YuckQi.Data.Sorting.Abstract
{
    public interface ISortExpression<T> where T : class
    {
        SortCriteria<T> Criteria { get; }

        string GetSortExpression();
    }
}