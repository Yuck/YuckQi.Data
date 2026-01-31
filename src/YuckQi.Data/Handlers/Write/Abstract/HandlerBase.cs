using YuckQi.Data.Handlers.Internal;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Handlers.Write.Abstract;

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

    protected virtual TData? MapToData(TEntity? entity)
    {
        return DataMapper.Default.MapToTarget<TEntity, TData>(entity, Mapper);
    }

    protected virtual IReadOnlyCollection<TData> MapToDataCollection(IEnumerable<TEntity>? entities)
    {
        return DataMapper.Default.MapToTargetCollection<TEntity, TData>(entities, Mapper);
    }
}
