using System;
using System.Collections.Generic;
using System.Data;
using YuckQi.Data.Sql.Dapper.Abstract;
using YuckQi.Data.Sql.Dapper.Handlers.Abstract;
using YuckQi.Data.Sql.Dapper.Oracle.Internal;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.Oracle.Handlers
{
    public class SearchHandler<TEntity, TKey, TScope, TRecord> : SearchHandlerBase<TEntity, TKey, TScope, TRecord> where TEntity : IEntity<TKey> where TKey : struct where TScope : IDbTransaction
    {
        public SearchHandler(ISqlGenerator<TRecord> sqlGenerator) : base(sqlGenerator, DbTypeMap.Default) { }

        public SearchHandler(ISqlGenerator<TRecord> sqlGenerator, IReadOnlyDictionary<Type, DbType> dbTypeMap) : base(sqlGenerator, dbTypeMap) { }
    }
}
