using System.Data;
using Dapper;
using YuckQi.Data.Filtering;

namespace YuckQi.Data.Sql.Dapper.Extensions;

public static class DynamicParameterExtensions
{
    public static DynamicParameters ToDynamicParameters(this IEnumerable<FilterCriteria>? parameters, IReadOnlyDictionary<Type, DbType>? dbTypeMap = null)
    {
        var result = new DynamicParameters();

        if (parameters == null)
            return result;

        foreach (var parameter in parameters)
        {
            var name = parameter.FieldName;
            var value = parameter.Value;
            var type = value?.GetType();
            var dbType = dbTypeMap != null && type != null && dbTypeMap.TryGetValue(type, out var mapped) ? (DbType?) mapped : null;

            result.Add(name, value, dbType);
        }

        return result;
    }
}
