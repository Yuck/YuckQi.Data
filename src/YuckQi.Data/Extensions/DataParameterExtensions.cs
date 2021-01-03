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
            { typeof(string), DbType.AnsiString },
            { typeof(bool), DbType.Boolean },
            { typeof(decimal), DbType.Decimal },
            { typeof(int), DbType.Int32 },
            { typeof(long), DbType.Int64 }
        };

        public static IReadOnlyCollection<T> ToParameterCollection<T>(this object parameters, IReadOnlyDictionary<Type, DbType> dbTypeMap = null) where T : IDataParameter, new()
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