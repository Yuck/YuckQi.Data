using YuckQi.Data.Handlers.Abstract;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.EntityFramework.Handlers;

public class ActivationHandler<TEntity, TIdentifier, TScope> : ActivationHandlerBase<TEntity, TIdentifier, TScope> where TIdentifier : struct where TEntity : IEntity<TIdentifier>, IActivated, IRevised
{
    public ActivationHandler(IRevisionHandler<TEntity, TIdentifier, TScope> reviser) : base(reviser) { }
}
