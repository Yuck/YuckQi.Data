using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Handlers.Abstract;

public abstract class ReadHandlerBase<TEntity>
{
    #region Properties

    protected IMapper? Mapper { get; }

    #endregion


    #region Constructors

    protected ReadHandlerBase(IMapper? mapper)
    {
        Mapper = mapper;
    }

    #endregion


    #region Protected Methods

    protected TEntity? MapToEntity<T>(T? data)
    {
        return data switch
        {
            null => default,
            TEntity entity => entity,
            _ => Mapper != null
                     ? Mapper.Map<TEntity>(data)
                     : throw new InvalidOperationException($"Unable to map '{typeof(T).Name}' to {typeof(TEntity).Name}; {nameof(Mapper)} instance is null.")
        };
    }

    protected IReadOnlyCollection<TEntity> MapToEntityCollection<T>(IEnumerable<T>? data)
    {
        return data switch
        {
            null => Array.Empty<TEntity>(),
            IEnumerable<TEntity> entities => entities.ToList(),
            _ => Mapper != null
                     ? Mapper.Map<IReadOnlyCollection<TEntity>>(data)
                     : throw new InvalidOperationException($"Unable to map '{typeof(IEnumerable<T>).Name}' to {typeof(IEnumerable<TEntity>).Name}; {nameof(Mapper)} instance is null.")
        };
    }

    #endregion
}
