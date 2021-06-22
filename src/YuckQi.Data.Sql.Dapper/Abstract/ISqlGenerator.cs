using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using YuckQi.Data.Sorting;
using YuckQi.Domain.ValueObjects.Abstract;

namespace YuckQi.Data.Sql.Dapper.Abstract
{
    public interface ISqlGenerator<in TDataParameter> where TDataParameter : IDataParameter
    {
        String GenerateCountQuery(IReadOnlyCollection<TDataParameter> parameters);
        String GenerateGetQuery(IReadOnlyCollection<TDataParameter> parameters);
        String GenerateSearchQuery(IReadOnlyCollection<TDataParameter> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort);
    }
}