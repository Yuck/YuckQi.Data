namespace YuckQi.Data.Sorting.Abstract
{
    public interface ISortExpression
    {
        SortCriteria Criteria { get; }

        string GetSortExpression();
    }
}