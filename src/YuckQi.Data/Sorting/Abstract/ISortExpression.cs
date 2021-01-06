namespace YuckQi.Data.Sorting.Abstract
{
    public interface ISortExpression
    {
        string Expression { get; set; }
        SortOrder Order { get; set; }

        string GetSortExpression();
    }
}