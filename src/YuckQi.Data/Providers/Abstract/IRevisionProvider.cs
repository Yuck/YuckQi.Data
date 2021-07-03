using System.Data;
using System.Threading.Tasks;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Providers.Abstract
{
    public interface IRevisionProvider<TEntity, TKey> where TEntity : IEntity<TKey>, IRevised where TKey : struct
    {
        TEntity Revise(TEntity entity, IDbTransaction transaction);
        Task<TEntity> ReviseAsync(TEntity entity, IDbTransaction transaction);
    }
}