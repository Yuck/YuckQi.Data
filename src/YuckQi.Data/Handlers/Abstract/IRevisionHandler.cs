using System.Threading.Tasks;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Handlers.Abstract
{
    public interface IRevisionHandler<TEntity, TKey, in TScope> where TEntity : IEntity<TKey>, IRevised where TKey : struct
    {
        TEntity Revise(TEntity entity, TScope scope);

        Task<TEntity> ReviseAsync(TEntity entity, TScope scope);
    }
}
