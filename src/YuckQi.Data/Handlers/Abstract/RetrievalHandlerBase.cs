using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YuckQi.Data.Extensions;
using YuckQi.Data.Filtering;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Handlers.Abstract
{
    public abstract class RetrievalHandlerBase<TEntity, TKey, TScope> : IRetrievalHandler<TEntity, TKey, TScope> where TEntity : IEntity<TKey> where TKey : struct
    {
        #region Properties

        protected IMapper Mapper { get; }

        #endregion


        #region Constructors

        protected RetrievalHandlerBase(IMapper mapper)
        {
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        #endregion


        #region Public Methods

        public TEntity Get(TKey key, TScope scope)
        {
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            return DoGet(key, scope);
        }

        public Task<TEntity> GetAsync(TKey key, TScope scope)
        {
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            return DoGetAsync(key, scope);
        }

        public TEntity Get(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            return DoGet(parameters, scope);
        }

        public Task<TEntity> GetAsync(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            return DoGetAsync(parameters, scope);
        }

        public TEntity Get(Object parameters, TScope scope)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            return Get(parameters.ToFilterCollection(), scope);
        }

        public Task<TEntity> GetAsync(Object parameters, TScope scope)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            return GetAsync(parameters.ToFilterCollection(), scope);
        }

        public IReadOnlyCollection<TEntity> GetList(TScope scope)
        {
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            return DoGetList(null, scope);
        }

        public Task<IReadOnlyCollection<TEntity>> GetListAsync(TScope scope)
        {
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            return DoGetListAsync(null, scope);
        }

        public IReadOnlyCollection<TEntity> GetList(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            return DoGetList(parameters, scope);
        }

        public Task<IReadOnlyCollection<TEntity>> GetListAsync(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            return DoGetListAsync(parameters, scope);
        }

        public IReadOnlyCollection<TEntity> GetList(Object parameters, TScope scope) => GetList(parameters?.ToFilterCollection(), scope);

        public Task<IReadOnlyCollection<TEntity>> GetListAsync(Object parameters, TScope scope) => GetListAsync(parameters?.ToFilterCollection(), scope);

        #endregion


        #region Protected Methods

        protected abstract TEntity DoGet(TKey key, TScope scope);

        protected abstract Task<TEntity> DoGetAsync(TKey key, TScope scope);

        protected abstract TEntity DoGet(IReadOnlyCollection<FilterCriteria> parameters, TScope scope);

        protected abstract Task<TEntity> DoGetAsync(IReadOnlyCollection<FilterCriteria> parameters, TScope scope);

        protected abstract IReadOnlyCollection<TEntity> DoGetList(IReadOnlyCollection<FilterCriteria> parameters, TScope scope);

        protected abstract Task<IReadOnlyCollection<TEntity>> DoGetListAsync(IReadOnlyCollection<FilterCriteria> parameters, TScope scope);

        #endregion
    }
}
