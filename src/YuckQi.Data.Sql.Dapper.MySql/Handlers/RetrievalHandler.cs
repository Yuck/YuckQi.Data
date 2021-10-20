using System;
using System.Collections.Generic;
using System.Data;
using YuckQi.Data.Sql.Dapper.Abstract;
using YuckQi.Data.Sql.Dapper.Handlers.Abstract;
using YuckQi.Data.Sql.Dapper.MySql.Internal;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.MySql.Handlers
{
    public class RetrievalHandler<TEntity, TKey, TScope, TRecord> : RetrievalHandlerBase<TEntity, TKey, TScope, TRecord> where TEntity : IEntity<TKey> where TKey : struct where TScope : IDbTransaction
    {
        public RetrievalHandler(ISqlGenerator<TRecord> sqlGenerator) : base(sqlGenerator, DbTypeMap.Default) { }

        public RetrievalHandler(ISqlGenerator<TRecord> sqlGenerator, IReadOnlyDictionary<Type, DbType> dbTypeMap) : base(sqlGenerator, dbTypeMap) { }
    }
}
