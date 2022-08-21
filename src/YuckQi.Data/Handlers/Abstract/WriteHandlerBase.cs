﻿using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Handlers.Abstract;

public abstract class WriteHandlerBase<TEntity>
{
    #region Properties

    protected IMapper? Mapper { get; }

    #endregion


    #region Constructors

    protected WriteHandlerBase(IMapper? mapper)
    {
        Mapper = mapper;
    }

    #endregion


    #region Protected Methods

    protected T? MapToData<T>(TEntity? entity)
    {
        return entity switch
        {
            null => default,
            T data => data,
            _ => Mapper != null
                     ? Mapper.Map<T>(entity)
                     : throw new InvalidOperationException($"Unable to map '{typeof(TEntity).Name}' to {typeof(T).Name}; {nameof(Mapper)} instance is null.")
        };
    }

    #endregion
}