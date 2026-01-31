using YuckQi.Data.Handlers.Internal;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Handlers.Abstract;

public abstract class ReadHandlerBase<TEntity> : ReadHandlerBase<TEntity, TEntity>
{
    protected ReadHandlerBase(IMapper? mapper) : base(mapper) { }
}

public abstract class ReadHandlerBase<TEntity, TData>
{
    protected IMapper? Mapper { get; }

    protected ReadHandlerBase(IMapper? mapper)
    {
        Mapper = mapper;
    }

    protected virtual TEntity? MapToEntity(TData? data)
    {
        return DataMapper.MapToTarget<TData, TEntity>(data, Mapper);
    }

    protected virtual TEntity? MapToEntity<T>(T? data)
    {
        return DataMapper.MapToTarget<T, TEntity>(data, Mapper);
    }

    protected virtual IReadOnlyCollection<TEntity> MapToEntityCollection(IEnumerable<TData>? data)
    {
        return DataMapper.MapToTargetCollection<TData, TEntity>(data, Mapper);
    }

    protected virtual IReadOnlyCollection<TEntity> MapToEntityCollection<T>(IEnumerable<T>? data)
    {
        return DataMapper.MapToTargetCollection<T, TEntity>(data, Mapper);
    }
}
