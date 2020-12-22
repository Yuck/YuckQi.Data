using YuckQi.Data.Abstract;

namespace YuckQi.Data.Repositories.Abstract
{
    // TODO: What can this interface really provide? UOW features?
    internal interface IEntityRepository<TEntity, TKey> where TEntity : IEntity<TKey> where TKey : struct
    {
        // dictionary of handlers with the type as the key? but this limits having only one configured...is that okay?
    }
}