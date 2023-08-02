using System.Data;
using YuckQi.Data.Sql.Dapper.Abstract;
using YuckQi.Data.Sql.Dapper.Handlers.Abstract;
using YuckQi.Data.Sql.Dapper.SqlServer.Internal;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Sql.Dapper.SqlServer.Handlers;

public class SearchHandler<TEntity, TIdentifier, TScope> : SearchHandler<TEntity, TIdentifier, TScope?, TEntity> where TEntity : IEntity<TIdentifier> where TIdentifier : struct, IEquatable<TIdentifier> where TScope : IDbTransaction?
{
    public SearchHandler() : this(new SqlGenerator<TEntity>()) { }

    public SearchHandler(ISqlGenerator sqlGenerator) : this(sqlGenerator, DbTypeMap.Default) { }

    public SearchHandler(ISqlGenerator sqlGenerator, IReadOnlyDictionary<Type, DbType> dbTypeMap) : base(sqlGenerator, dbTypeMap, null) { }
}

public class SearchHandler<TEntity, TIdentifier, TScope, TRecord> : SearchHandlerBase<TEntity, TIdentifier, TScope?, TRecord> where TEntity : IEntity<TIdentifier> where TIdentifier : struct, IEquatable<TIdentifier> where TScope : IDbTransaction?
{
    public SearchHandler(IMapper mapper) : this(new SqlGenerator<TRecord>(), mapper) { }

    public SearchHandler(ISqlGenerator sqlGenerator, IMapper mapper) : this(sqlGenerator, DbTypeMap.Default, mapper) { }

    public SearchHandler(ISqlGenerator sqlGenerator, IReadOnlyDictionary<Type, DbType> dbTypeMap, IMapper? mapper) : base(sqlGenerator, dbTypeMap, mapper) { }
}
