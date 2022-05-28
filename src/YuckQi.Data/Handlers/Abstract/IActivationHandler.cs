using System.Threading.Tasks;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Handlers.Abstract;

public interface IActivationHandler<TEntity, in TIdentifier, in TScope> where TEntity : IEntity<TIdentifier>, IActivated, IRevised where TIdentifier : struct
{
    TEntity Activate(TEntity entity, TScope scope);

    Task<TEntity> ActivateAsync(TEntity entity, TScope scope);

    TEntity Deactivate(TEntity entity, TScope scope);

    Task<TEntity> DeactivateAsync(TEntity entity, TScope scope);
}
