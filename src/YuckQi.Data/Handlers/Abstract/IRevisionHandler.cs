using System.Threading.Tasks;
using YuckQi.Data.Entities.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Handlers.Abstract
{
    public interface IRevisionHandler<TEntity, TKey> where TEntity : IEntity<TKey>, IRevised where TKey : struct
    {
        Task<TEntity> ReviseAsync(TEntity entity);
    }
}