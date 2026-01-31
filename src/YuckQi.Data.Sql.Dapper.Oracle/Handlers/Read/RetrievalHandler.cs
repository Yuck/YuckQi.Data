using System.Data;
using YuckQi.Data.Sql.Dapper.Abstract.Interfaces;
using YuckQi.Data.Sql.Dapper.Handlers.Read.Abstract;
using YuckQi.Data.Sql.Dapper.Oracle.Internal;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Sql.Dapper.Oracle.Handlers.Read;

public class RetrievalHandler<TEntity, TIdentifier, TScope> : RetrievalHandler<TEntity, TIdentifier, TScope?, TEntity> where TEntity : IEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier> where TScope : IDbTransaction?
{
    public RetrievalHandler() : this(new SqlGenerator<TEntity>()) { }

    public RetrievalHandler(ISqlGenerator sqlGenerator) : this(sqlGenerator, DbTypeMap.Default) { }

    public RetrievalHandler(ISqlGenerator sqlGenerator, IReadOnlyDictionary<Type, DbType> dbTypeMap) : base(sqlGenerator, dbTypeMap, null) { }
}

public class RetrievalHandler<TEntity, TIdentifier, TScope, TRecord> : RetrievalHandlerBase<TEntity, TIdentifier, TScope?, TRecord> where TEntity : IEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier> where TScope : IDbTransaction?
{
    public RetrievalHandler(IMapper mapper) : this(new SqlGenerator<TRecord>(), mapper) { }

    public RetrievalHandler(ISqlGenerator sqlGenerator, IMapper mapper) : this(sqlGenerator, DbTypeMap.Default, mapper) { }

    public RetrievalHandler(ISqlGenerator sqlGenerator, IReadOnlyDictionary<Type, DbType> dbTypeMap, IMapper? mapper) : base(sqlGenerator, dbTypeMap, mapper) { }
}
