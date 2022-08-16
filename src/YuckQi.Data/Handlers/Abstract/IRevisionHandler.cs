using System.Threading;
using System.Threading.Tasks;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Handlers.Abstract;

public interface IRevisionHandler<TEntity, TIdentifier, in TScope> where TEntity : IEntity<TIdentifier>, IRevised where TIdentifier : struct
{
    TEntity Revise(TEntity entity, TScope scope);

    Task<TEntity> Revise(TEntity entity, TScope scope, CancellationToken cancellationToken);
}
