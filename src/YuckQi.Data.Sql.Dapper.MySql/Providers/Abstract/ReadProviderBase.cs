using YuckQi.Data.Abstract;
using YuckQi.Data.Sql.Dapper.Providers.Abstract;

namespace YuckQi.Data.Sql.Dapper.MySql.Providers.Abstract
{
    public abstract class ReadProviderBase<TRecord> : DataProviderBase
    {
        #region Constructors

        protected ReadProviderBase(IUnitOfWork context) : base(context) { }

        #endregion
    }
}