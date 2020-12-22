using System.Threading.Tasks;
using YuckQi.Data.Abstract;

namespace YuckQi.Data.Handlers.Abstract
{
    internal interface IActivationHandler<TEntity, in TKey> where TEntity : IEntity<TKey> where TKey : struct, IActivated
    {
        // TODO: What should these return? bool? The entity?
        Task ActivateAsync(TKey key);
        Task DeactivateAsync(TKey key);
    }
}