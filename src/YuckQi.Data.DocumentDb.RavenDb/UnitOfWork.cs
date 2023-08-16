using Raven.Client.Documents.Session;
using YuckQi.Data.Abstract;

namespace YuckQi.Data.DocumentDb.RavenDb;

public class UnitOfWork : IUnitOfWork<IDocumentSession>
{
    public IDocumentSession? Scope { get; private set; }

    public UnitOfWork(IDocumentSession context)
    {
        Scope = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void Dispose()
    {
        if (Scope == null)
            return;

        Scope?.Dispose();

        Scope = null;
    }

    public void SaveChanges()
    {
        if (Scope == null)
            throw new InvalidOperationException();

        Scope.SaveChanges();
    }
}
