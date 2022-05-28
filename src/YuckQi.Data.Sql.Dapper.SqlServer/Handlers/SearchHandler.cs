using System;
using System.Collections.Generic;
using System.Data;
using YuckQi.Data.Sql.Dapper.Abstract;
using YuckQi.Data.Sql.Dapper.Handlers.Abstract;
using YuckQi.Data.Sql.Dapper.SqlServer.Internal;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Sql.Dapper.SqlServer.Handlers;

public class SearchHandler<TEntity, TIdentifier, TScope, TRecord> : SearchHandlerBase<TEntity, TIdentifier, TScope, TRecord> where TEntity : IEntity<TIdentifier> where TIdentifier : struct where TScope : IDbTransaction
{
    public SearchHandler(IMapper mapper) : this(mapper, new SqlGenerator<TRecord>()) { }

    public SearchHandler(IMapper mapper, ISqlGenerator<TRecord> sqlGenerator) : this(mapper, sqlGenerator, DbTypeMap.Default) { }

    public SearchHandler(IMapper mapper, ISqlGenerator<TRecord> sqlGenerator, IReadOnlyDictionary<Type, DbType> dbTypeMap) : base(mapper, sqlGenerator, dbTypeMap) { }
}
