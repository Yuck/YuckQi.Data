using System.Data;
using System.Threading.Tasks;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Providers.Abstract
{
    public interface ICreationProvider<TEntity, TKey> where TEntity : IEntity<TKey>, ICreated where TKey : struct
    {
        TEntity Create(TEntity entity, IDbTransaction transaction);
        Task<TEntity> CreateAsync(TEntity entity, IDbTransaction transaction);
    }
}