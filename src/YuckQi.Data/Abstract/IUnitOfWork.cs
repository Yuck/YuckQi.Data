using System;
using System.Data;

namespace YuckQi.Data.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IDbConnection Db { get; }
        IDbTransaction Transaction { get; }

        void SaveChanges();
    }
}