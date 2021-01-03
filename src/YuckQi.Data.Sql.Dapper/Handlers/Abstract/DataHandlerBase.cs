using System;
using System.Data;
using YuckQi.Data.Abstract;

namespace YuckQi.Data.Sql.Dapper.Handlers.Abstract
{
    public abstract class DataHandlerBase
    {
        #region Private Members

        private readonly IUnitOfWork _uow;

        #endregion


        #region Properties

        protected IDbConnection Db => _uow.Db;
        protected IDbTransaction Transaction => _uow.Transaction;

        #endregion


        #region Constructors

        protected DataHandlerBase(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        #endregion
    }
}