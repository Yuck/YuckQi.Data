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
            { typeof(Boolean), DbType.Boolean },
            { typeof(Byte), DbType.Byte },
            { typeof(DateTime), DbType.DateTime2 },
            { typeof(DateTimeOffset), DbType.DateTimeOffset },
            { typeof(Decimal), DbType.Decimal },
            { typeof(Double), DbType.Double },
            { typeof(Single), DbType.Single },
            { typeof(Guid), DbType.Guid },
            { typeof(Int32), DbType.Int32 },
            { typeof(Int64), DbType.Int64 },
            { typeof(String), DbType.AnsiString }
        };

        public static IReadOnlyCollection<TDataParameter> ToParameterCollection<TDataParameter>(this Object parameters, IReadOnlyDictionary<Type, DbType> dbTypeMap = null) where TDataParameter : IDataParameter, new()
        {
            if (parameters == null)
                return new List<TDataParameter>();

            return parameters.GetType().GetProperties().Select(t =>
            {
                var name = t.Name;
                var value = t.GetValue(parameters);
                var map = dbTypeMap ?? DbTypeMap;
                var type = value.GetType();

                return new TDataParameter
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