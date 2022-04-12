using System;

namespace YuckQi.Data.Exceptions;

public sealed class PhysicalDeletionException<TRecord, TKey> : ApplicationException where TKey : struct
{
    #region Constructors

    public PhysicalDeletionException(TKey key) : base(GetMessageText(key)) { }

    public PhysicalDeletionException(TKey key, Exception inner) : base(GetMessageText(key), inner) { }

    #endregion


    #region Supporting Methods

    private static String GetMessageText(TKey key) => $"Failed to delete '{nameof(TRecord)}' with key '{key}'.";

    #endregion
}
