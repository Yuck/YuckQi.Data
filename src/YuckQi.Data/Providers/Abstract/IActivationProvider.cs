using System.Threading.Tasks;
using YuckQi.Data.Entities.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Providers.Abstract
{
    public interface IActivationProvider<TEntity, in TKey> where TEntity : IEntity<TKey>, IActivated where TKey : struct
    {
        Task<TEntity> ActivateAsync(TEntity entity);
        Task<TEntity> DeactivateAsync(TEntity entity);
    }
}