namespace YuckQi.Data.Abstract.Interfaces;

public interface IUnitOfWork<out TScope> : IDisposable
{
    TScope? Scope { get; }

    void SaveChanges();
}
