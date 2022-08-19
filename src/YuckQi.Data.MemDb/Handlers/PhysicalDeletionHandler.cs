﻿using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.MemDb.Handlers;

public class PhysicalDeletionHandler<TEntity, TIdentifier, TScope> : PhysicalDeletionHandlerBase<TEntity, TIdentifier, TScope> where TEntity : IEntity<TIdentifier> where TIdentifier : struct
{
    private readonly ConcurrentDictionary<TIdentifier, TEntity> _entities;

    public PhysicalDeletionHandler(ConcurrentDictionary<TIdentifier, TEntity> entities)
    {
        _entities = entities ?? throw new ArgumentNullException(nameof(entities));
    }

    protected override Boolean DoDelete(TEntity entity, TScope scope) => _entities.TryRemove(entity.Identifier, out _);

    protected override Task<Boolean> DoDelete(TEntity entity, TScope scope, CancellationToken cancellationToken) => Task.FromResult(DoDelete(entity, scope));
}
