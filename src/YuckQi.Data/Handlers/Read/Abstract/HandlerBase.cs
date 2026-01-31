using YuckQi.Data.Handlers.Internal;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Handlers.Read.Abstract;

public abstract class HandlerBase<TEntity> : HandlerBase<TEntity, TEntity>
{
    protected HandlerBase(IMapper? mapper) : base(mapper) { }
}

public abstract class HandlerBase<TEntity, TData>
{
    protected IMapper? Mapper { get; }

    protected HandlerBase(IMapper? mapper)
    {
        Mapper = mapper;
    }

    protected virtual TEntity? MapToEntity(TData? data)
    {
        return DataMapper.Default.MapToTarget<TData, TEntity>(data, Mapper);
    }

    protected virtual IReadOnlyCollection<TEntity> MapToEntityCollection(IEnumerable<TData>? data)
    {
        return DataMapper.Default.MapToTargetCollection<TData, TEntity>(data, Mapper);
    }
}
