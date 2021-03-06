﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using YuckQi.Data.Sorting;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Domain.ValueObjects.Abstract;

namespace YuckQi.Data.Providers.Abstract
{
    public interface ISearchProvider<TEntity, TKey, in TDataParameter> where TEntity : IEntity<TKey> where TKey : struct where TDataParameter : IDataParameter
    {
        IPage<TEntity> Search(IReadOnlyCollection<TDataParameter> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, IDbTransaction transaction);
        Task<IPage<TEntity>> SearchAsync(IReadOnlyCollection<TDataParameter> parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, IDbTransaction transaction);

        IPage<TEntity> Search(Object parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, IDbTransaction transaction);
        Task<IPage<TEntity>> SearchAsync(Object parameters, IPage page, IOrderedEnumerable<SortCriteria> sort, IDbTransaction transaction);
    }
}