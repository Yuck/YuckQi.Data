using System;
using System.Collections.Generic;
using System.Data;
using YuckQi.Data.Sql.Dapper.Abstract;
using YuckQi.Data.Sql.Dapper.Handlers.Abstract;
using YuckQi.Data.Sql.Dapper.SqlServer.Internal;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Sql.Dapper.SqlServer.Handlers;

public class RetrievalHandler<TEntity, TIdentifier, TScope, TRecord> : RetrievalHandlerBase<TEntity, TIdentifier, TScope, TRecord> where TEntity : IEntity<TIdentifier> where TIdentifier : struct where TScope : IDbTransaction
{
    public RetrievalHandler(IMapper mapper) : this(mapper, new SqlGenerator<TRecord>()) { }

    public RetrievalHandler(IMapper mapper, ISqlGenerator<TRecord> sqlGenerator) : this(mapper, sqlGenerator, DbTypeMap.Default) { }

    public RetrievalHandler(IMapper mapper, ISqlGenerator<TRecord> sqlGenerator, IReadOnlyDictionary<Type, DbType> dbTypeMap) : base(mapper, sqlGenerator, dbTypeMap) { }
}
