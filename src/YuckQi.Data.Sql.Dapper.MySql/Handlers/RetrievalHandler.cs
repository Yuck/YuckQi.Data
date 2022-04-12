using System;
using System.Collections.Generic;
using System.Data;
using YuckQi.Data.Sql.Dapper.Abstract;
using YuckQi.Data.Sql.Dapper.Handlers.Abstract;
using YuckQi.Data.Sql.Dapper.MySql.Internal;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Sql.Dapper.MySql.Handlers;

public class RetrievalHandler<TEntity, TKey, TScope, TRecord> : RetrievalHandlerBase<TEntity, TKey, TScope, TRecord> where TEntity : IEntity<TKey> where TKey : struct where TScope : IDbTransaction
{
    public RetrievalHandler(IMapper mapper) : this(mapper, new SqlGenerator<TRecord>()) { }

    public RetrievalHandler(IMapper mapper, ISqlGenerator<TRecord> sqlGenerator) : this(mapper, sqlGenerator, DbTypeMap.Default) { }

    public RetrievalHandler(IMapper mapper, ISqlGenerator<TRecord> sqlGenerator, IReadOnlyDictionary<Type, DbType> dbTypeMap) : base(mapper, sqlGenerator, dbTypeMap) { }
}
