using System;
using System.Collections.Generic;
using System.Data;
using YuckQi.Data.Sql.Dapper.Abstract;
using YuckQi.Data.Sql.Dapper.Handlers.Abstract;
using YuckQi.Data.Sql.Dapper.MySql.Internal;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Sql.Dapper.MySql.Handlers
{
    public class SearchHandler<TEntity, TKey, TScope, TRecord> : SearchHandlerBase<TEntity, TKey, TScope, TRecord> where TEntity : IEntity<TKey> where TKey : struct where TScope : IDbTransaction
    {
        public SearchHandler(ISqlGenerator<TRecord> sqlGenerator, IMapper mapper) : base(sqlGenerator, DbTypeMap.Default, mapper) { }

        public SearchHandler(ISqlGenerator<TRecord> sqlGenerator, IReadOnlyDictionary<Type, DbType> dbTypeMap, IMapper mapper) : base(sqlGenerator, dbTypeMap, mapper) { }
    }
}
