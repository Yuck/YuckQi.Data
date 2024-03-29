﻿using System.Collections.ObjectModel;
using System.Data;

namespace YuckQi.Data.Sql.Dapper.Oracle.Internal;

internal sealed class DbTypeMap : ReadOnlyDictionary<Type, DbType>
{
    private static readonly Lazy<DbTypeMap> DefaultInstance = new(() => new DbTypeMap(new Dictionary<Type, DbType>
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
    }));

    public static DbTypeMap Default => DefaultInstance.Value;

    public DbTypeMap(IDictionary<Type, DbType> dictionary) : base(dictionary) { }
}
