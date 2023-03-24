using System.Collections;
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
            if (parameter.Operation == FilterOperation.In)
            {
                var set = parameter.Value is IEnumerable enumerable
                              ? enumerable.Cast<Object>().ToArray()
                              : throw new ArgumentException($"{nameof(parameter.Value)} must be convertible to {nameof(IEnumerable)}.");

                for (var i = 0; i < set.Length; i++)
                {
                    var name = $"{parameter.FieldName}{i}";
                    var value = set[i];
                    var type = value?.GetType();
                    var dbType = dbTypeMap != null && type != null && dbTypeMap.TryGetValue(type, out var mapped) ? (DbType?) mapped : null;

                    result.Add(name, value, dbType);
                }
            }
            else
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
