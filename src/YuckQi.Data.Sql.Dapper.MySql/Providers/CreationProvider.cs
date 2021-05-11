using System;
using System.Threading.Tasks;
using YuckQi.Data.Abstract;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Data.Sql.Dapper.Providers.Abstract;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.MySql.Providers
{
    public class CreationProvider<TEntity, TKey, TRecord> : DataProviderBase, ICreationProvider<TEntity, TKey> where TEntity : IEntity<TKey>, ICreated where TKey : struct
    {
        #region Constructors

        public CreationProvider(IUnitOfWork context) : base(context) { }

        #endregion


        #region Public Methods

        public Task<TEntity> CreateAsync(TEntity entity) => throw new NotImplementedException();

        #endregion
    }
}