using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace YuckQi.Data.Extensions
{
    public static class DataParameterExtensions
    {
        private static readonly IReadOnlyDictionary<Type, DbType> DbTypeMap = new Dictionary<Type, DbType>
        {
            {typeof(Boolean), DbType.Boolean},
            {typeof(Byte), DbType.Byte},
            {typeof(DateTime), DbType.DateTime2},
            {typeof(DateTimeOffset), DbType.DateTimeOffset},
            {typeof(Decimal), DbType.Decimal},
            {typeof(Double), DbType.Double},
            {typeof(Single), DbType.Single},
            {typeof(Guid), DbType.Guid},
            {typeof(Int32), DbType.Int32},
            {typeof(Int64), DbType.Int64},
            {typeof(String), DbType.AnsiString}
        };

        public static IReadOnlyCollection<T> ToParameterCollection<T>(this Object parameters, IReadOnlyDictionary<Type, DbType> dbTypeMap = null) where T : IDataParameter, new()
        {
            if (parameters == null)
                return new List<T>();

            return parameters.GetType().GetProperties().Select(t =>
            {
                var name = t.Name;
                var value = t.GetValue(parameters);
                var map = dbTypeMap ?? DbTypeMap;
                var type = value.GetType();

                return new T
                {
                    DbType = map[type],
                    Direction = ParameterDirection.Input,
                    ParameterName = name,
                    Value = value
                };
            }).ToList();
        }
    }
}