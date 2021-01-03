using System.Threading.Tasks;
using YuckQi.Data.Entities.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Handlers.Abstract
{
    public interface ICreationHandler<TEntity, TKey> where TEntity : IEntity<TKey>, ICreated where TKey : struct
    {
        Task<TEntity> CreateAsync(TEntity entity);
    }
}