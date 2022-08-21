namespace YuckQi.Data.Abstract;

public interface IUnitOfWork<out TScope> : IDisposable
{
    TScope Scope { get; }

    void SaveChanges();
}
