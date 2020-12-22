using System.Threading.Tasks;

namespace YuckQi.Data.Repositories.Abstract
{
    internal interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}