using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Handlers.Internal;

internal static class DataMapper
{
    public static TTarget? MapToTarget<TSource, TTarget>(TSource? source, IMapper? mapper)
    {
        return source switch
        {
            null => default,
            TTarget entity => entity,
            _ => mapper != null
                     ? mapper.Map<TTarget>(source)
                     : throw new InvalidOperationException($"Unable to map '{typeof(TSource).Name}' to {typeof(TTarget).Name}; {nameof(mapper)} instance is null.")
        };
    }

    public static IReadOnlyCollection<TTarget> MapToTargetCollection<TSource, TTarget>(IEnumerable<TSource>? source, IMapper? mapper)
    {
        return source switch
        {
            null => [],
            IEnumerable<TTarget> entities => [.. entities],
            _ => mapper != null
                     ? mapper.Map<IReadOnlyCollection<TTarget>>(source)
                     : throw new InvalidOperationException($"Unable to map '{typeof(IEnumerable<TSource>).Name}' to {typeof(IEnumerable<TTarget>).Name}; {nameof(mapper)} instance is null.")
        };
    }
}
