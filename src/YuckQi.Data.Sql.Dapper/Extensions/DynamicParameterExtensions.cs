using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using YuckQi.Data.Filtering;

namespace YuckQi.Data.Sql.Dapper.Extensions
{
    public static class DynamicParameterExtensions
    {
        private static readonly IReadOnlyDictionary<Type, DbType> DbTypeMap = new Dictionary<Type, DbType>
        {
            { typeof(Boolean), DbType.Boolean },
            { typeof(Byte), DbType.Byte },
            { typeof(DateTime), DbType.DateTime2 },
            { typeof(DateTimeOffset), DbType.DateTimeOffset },
            { typeof(Decimal), DbType.Decimal },
            { typeof(Double), DbType.Double },
            { typeof(Guid), DbType.Guid },
            { typeof(Int32), DbType.Int32 },
            { typeof(Int64), DbType.Int64 },
            { typeof(Single), DbType.Single },
            { typeof(String), DbType.AnsiString }
        };

        public static DynamicParameters ToDynamicParameters(this IEnumerable<FilterCriteria> parameters, IReadOnlyDictionary<Type, DbType> dbTypeMap = null)
        {
            if (parameters == null)
                return null;

            var map = dbTypeMap ?? DbTypeMap;
            var result = new DynamicParameters();

            foreach (var parameter in parameters)
            {
                if (parameter.Operation != FilterOperation.Equal)
                    throw new NotSupportedException($"{nameof(DynamicParameters)} only supports {nameof(FilterOperation)} '{nameof(FilterOperation.Equal)}', but '{parameter.Operation}' was specified.");

                var name = parameter.FieldName;
                var value = parameter.Value;
                var type = value?.GetType();
                var dbType = type != null && map.TryGetValue(type, out var mapped) ? (DbType?) mapped : null;

                result.Add(name, value, dbType);
            }

            return result;
        }
    }
}
