using System;

namespace YuckQi.Data.Exceptions;

public sealed class PhysicalDeletionException<TEntity, TIdentifier> : ApplicationException where TIdentifier : struct
{
    #region Constructors

    public PhysicalDeletionException(TIdentifier identifier) : base(GetMessageText(identifier)) { }

    public PhysicalDeletionException(TIdentifier identifier, Exception inner) : base(GetMessageText(identifier), inner) { }

    #endregion


    #region Supporting Methods

    private static String GetMessageText(TIdentifier identifier) => $"Failed to delete '{typeof(TEntity).Name}' with identifier '{identifier}'.";

    #endregion
}
