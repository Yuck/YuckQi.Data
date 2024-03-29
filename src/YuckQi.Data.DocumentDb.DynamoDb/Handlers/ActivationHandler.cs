﻿using System;
using Amazon.DynamoDBv2.DataModel;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.DocumentDb.DynamoDb.Handlers;

public class ActivationHandler<TEntity, TIdentifier, TScope> : ActivationHandlerBase<TEntity, TIdentifier, TScope?> where TEntity : IEntity<TIdentifier>, IActivated, IRevised where TIdentifier : IEquatable<TIdentifier> where TScope : IDynamoDBContext?
{
    public ActivationHandler(IRevisionHandler<TEntity, TIdentifier, TScope?> reviser) : base(reviser) { }
}
