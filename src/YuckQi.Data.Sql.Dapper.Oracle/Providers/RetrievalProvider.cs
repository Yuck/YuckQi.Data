using System;
using System.Collections.Generic;
using System.Data;
using YuckQi.Data.Sql.Dapper.Abstract;
using YuckQi.Data.Sql.Dapper.Oracle.Internal;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.Oracle.Providers
{
    public class RetrievalProvider<TEntity, TKey, TScope, TRecord> : RetrievalProviderBase<TEntity, TKey, TScope, TRecord> where TEntity : IEntity<TKey> where TKey : struct where TScope : IDbTransaction
    {
        public RetrievalProvider(ISqlGenerator<TRecord> sqlGenerator) : base(sqlGenerator, DbTypeMap.Default) { }

        public RetrievalProvider(ISqlGenerator<TRecord> sqlGenerator, IReadOnlyDictionary<Type, DbType> dbTypeMap) : base(sqlGenerator, dbTypeMap) { }
    }
}
