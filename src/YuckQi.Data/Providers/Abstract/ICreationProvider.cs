using System.Threading.Tasks;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Providers.Abstract
{
    public interface ICreationProvider<TEntity, TKey, in TScope> where TEntity : IEntity<TKey>, ICreated where TKey : struct
    {
        TEntity Create(TEntity entity, TScope scope);

        Task<TEntity> CreateAsync(TEntity entity, TScope scope);
    }
}
