using System;
using System.Collections.Generic;
using System.Data;
using YuckQi.Data.Sql.Dapper.Abstract;
using YuckQi.Data.Sql.Dapper.Oracle.Internal;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.Oracle.Providers
{
    public class SearchProvider<TEntity, TKey, TScope, TRecord> : SearchProviderBase<TEntity, TKey, TScope, TRecord> where TEntity : IEntity<TKey> where TKey : struct where TScope : IDbTransaction
    {
        public SearchProvider(ISqlGenerator<TRecord> sqlGenerator) : base(sqlGenerator, DbTypeMap.Default) { }

        public SearchProvider(ISqlGenerator<TRecord> sqlGenerator, IReadOnlyDictionary<Type, DbType> dbTypeMap) : base(sqlGenerator, dbTypeMap) { }
    }
}
