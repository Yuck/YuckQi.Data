using MongoDB.Driver;
using YuckQi.Data.Handlers.Write.Abstract;
using YuckQi.Data.Handlers.Write.Abstract.Interfaces;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.DocumentDb.MongoDb.Handlers.Write;

public class ActivationHandler<TEntity, TIdentifier, TScope> : ActivationHandlerBase<TEntity, TIdentifier, TScope?> where TEntity : IEntity<TIdentifier>, IActivated, IRevised where TIdentifier : IEquatable<TIdentifier> where TScope : IClientSessionHandle?
{
    public ActivationHandler(IRevisionHandler<TEntity, TIdentifier, TScope?> reviser) : base(reviser) { }
}
