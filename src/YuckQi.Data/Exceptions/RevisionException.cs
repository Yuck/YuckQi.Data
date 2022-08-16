using System;

namespace YuckQi.Data.Exceptions;

public sealed class RevisionException<TEntity, TIdentifier> : ApplicationException where TIdentifier : struct
{
    #region Constructors

    public RevisionException(TIdentifier identifier) : base(GetMessageText(identifier)) { }

    public RevisionException(TIdentifier identifier, Exception inner) : base(GetMessageText(identifier), inner) { }

    #endregion


    #region Supporting Methods

    private static String GetMessageText(TIdentifier identifier) => $"Failed to revise '{typeof(TEntity).Name}' with identifier '{identifier}'.";

    #endregion
}
