using YuckQi.Data.Sorting;
using YuckQi.Data.Sorting.Abstract;

namespace YuckQi.Data.Sql.Dapper.Sorting
{
    public class SortExpression : ISortExpression<string>
    {
        public SortCriteria<string> Criteria { get; }

        public SortExpression(SortCriteria<string> criteria)
        {
            Criteria = criteria;
        }

        public string GetSortExpression()
        {
            return $"[{Criteria.Expression}] {(Criteria.Order == SortOrder.Descending ? "desc" : "asc")}";
        }
    }
}