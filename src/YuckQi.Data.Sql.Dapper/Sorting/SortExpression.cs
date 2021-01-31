using YuckQi.Data.Sorting;
using YuckQi.Data.Sorting.Abstract;

namespace YuckQi.Data.Sql.Dapper.Sorting
{
    public class SortExpression : ISortExpression
    {
        public SortCriteria Criteria { get; }

        public SortExpression(SortCriteria criteria)
        {
            Criteria = criteria;
        }

        public string GetSortExpression()
        {
            return $"[{Criteria.Expression}] {(Criteria.Order == SortOrder.Descending ? "desc" : "asc")}";
        }
    }
}