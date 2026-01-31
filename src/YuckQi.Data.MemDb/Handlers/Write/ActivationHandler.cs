using YuckQi.Data.Handlers.Write.Abstract;
using YuckQi.Data.Handlers.Write.Abstract.Interfaces;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.MemDb.Handlers.Write;

public class ActivationHandler<TEntity, TIdentifier, TScope> : ActivationHandlerBase<TEntity, TIdentifier, TScope?> where TEntity : IEntity<TIdentifier>, IActivated, IRevised where TIdentifier : IEquatable<TIdentifier>
{
    public ActivationHandler(IRevisionHandler<TEntity, TIdentifier, TScope?> reviser) : base(reviser) { }
}
