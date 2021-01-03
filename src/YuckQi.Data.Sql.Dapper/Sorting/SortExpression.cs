using System.Data.SqlClient;
using YuckQi.Data.Sorting.Abstract;

namespace YuckQi.Data.Sql.Dapper.Sorting
{
    public class SortExpression : ISortExpression
    {
        #region Properties

        public string Expression { get; set; }
        public SortOrder Order { get; set; }

        #endregion


        #region Constructors

        public string GetSortExpression()
        {
            return $"{Expression} {(Order == SortOrder.Descending ? "desc" : "asc")}";
        }

        #endregion
    }
}