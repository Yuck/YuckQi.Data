using System;
using System.Data;
using YuckQi.Data.Abstract;

namespace YuckQi.Data.Sql.Dapper.Providers.Abstract
{
    public abstract class DataProviderBase
    {
        #region Private Members

        private readonly IUnitOfWork _uow;

        #endregion


        #region Properties

        protected IDbConnection Db => _uow.Db;
        protected IDbTransaction Transaction => _uow.Transaction;

        #endregion


        #region Constructors

        protected DataProviderBase(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        #endregion
    }
}