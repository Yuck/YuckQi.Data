using System.Threading.Tasks;
using YuckQi.Data.Abstract;
using YuckQi.Data.Repositories.Abstract;

namespace YuckQi.Data.Handlers.Abstract
{
    internal interface IRevisionHandler<TEntity, TKey> where TEntity : IEntity<TKey> where TKey : struct, IRevised
    {
        Task<TEntity> ReviseAsync(TEntity entity, IUnitOfWork uow = null);
    }
}