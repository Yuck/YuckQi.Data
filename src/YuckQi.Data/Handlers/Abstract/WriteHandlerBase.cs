using YuckQi.Data.Handlers.Internal;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Handlers.Abstract;

public abstract class WriteHandlerBase<TEntity> : WriteHandlerBase<TEntity, TEntity>
{
    protected WriteHandlerBase(IMapper? mapper) : base(mapper) { }
}

public abstract class WriteHandlerBase<TEntity, TData>
{
    protected IMapper? Mapper { get; }

    protected WriteHandlerBase(IMapper? mapper)
    {
        Mapper = mapper;
    }

    protected virtual TData? MapToData(TEntity? entity)
    {
        return DataMapper.MapToTarget<TEntity, TData>(entity, Mapper);
    }

    protected virtual T? MapToData<T>(TEntity? entity)
    {
        return DataMapper.MapToTarget<TEntity, T>(entity, Mapper);
    }

    protected virtual IReadOnlyCollection<TData> MapToDataCollection(IEnumerable<TEntity>? entities)
    {
        return DataMapper.MapToTargetCollection<TEntity, TData>(entities, Mapper);
    }

    protected virtual IReadOnlyCollection<T> MapToDataCollection<T>(IEnumerable<TEntity>? entities)
    {
        return DataMapper.MapToTargetCollection<TEntity, T>(entities, Mapper);
    }
}
