using System;
using System.Threading.Tasks;
using YuckQi.Data.Abstract;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Data.Sql.Dapper.Providers.Abstract;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.MySql.Providers
{
    public class RevisionProvider<TEntity, TKey, TRecord> : DataProviderBase, IRevisionProvider<TEntity, TKey> where TEntity : IEntity<TKey>, IRevised where TKey : struct
    {
        #region Constructors

        public RevisionProvider(IUnitOfWork context) : base(context) { }

        #endregion


        #region Public Methods

        public Task<TEntity> ReviseAsync(TEntity entity) => throw new NotImplementedException();

        #endregion
    }
}