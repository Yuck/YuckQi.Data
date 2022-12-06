using Microsoft.EntityFrameworkCore;
using YuckQi.Data.Abstract;

namespace YuckQi.Data.Sql.EntityFramework;

public class UnitOfWork<TScope> : IUnitOfWork<TScope> where TScope : DbContext
{
    public TScope Scope { get; }

    public UnitOfWork(TScope scope)
    {
        Scope = scope ?? throw new ArgumentNullException(nameof(scope));
    }

    public void Dispose()
    {
        Scope.Dispose();
    }

    public void SaveChanges()
    {
        Scope.SaveChanges();
    }
}
