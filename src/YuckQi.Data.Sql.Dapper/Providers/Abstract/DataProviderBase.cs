using System;
using YuckQi.Data.Abstract;

namespace YuckQi.Data.Sql.Dapper.Providers.Abstract
{
    public abstract class DataProviderBase
    {
        #region Properties

        protected IUnitOfWork Context;

        #endregion


        #region Constructors

        protected DataProviderBase(IUnitOfWork context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #endregion
    }
}