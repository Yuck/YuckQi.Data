using System.Threading.Tasks;
using YuckQi.Data.Abstract;
using YuckQi.Data.Repositories.Abstract;

namespace YuckQi.Data.Handlers.Abstract
{
    internal interface ICreationHandler<TEntity, TKey> where TEntity : IEntity<TKey> where TKey : struct, ICreated
    {
        Task<TEntity> CreateAsync(TEntity entity, IUnitOfWork uow = null);
    }
}