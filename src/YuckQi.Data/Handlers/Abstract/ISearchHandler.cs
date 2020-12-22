using System.Collections.Generic;
using System.Threading.Tasks;
using YuckQi.Data.Abstract;
using YuckQi.Data.Repositories.Abstract;

namespace YuckQi.Data.Handlers.Abstract
{
    internal interface ISearchHandler<TEntity, TKey> where TEntity : IEntity<TKey> where TKey : struct
    {
        Task<IReadOnlyCollection<TEntity>> SearchAsync(object parameters, object page_criteria, IUnitOfWork uow = null); // TODO: As with IRetrievalHandler ... better way to collect this
    }
}