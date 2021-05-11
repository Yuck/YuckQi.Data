using System;

namespace YuckQi.Data.Exceptions
{
    public sealed class RecordDeleteException<TRecord, TKey> : ApplicationException where TKey : struct
    {
        #region Constructors

        public RecordDeleteException(TKey key) : base(GetMessageText(key)) { }

        public RecordDeleteException(TKey key, Exception inner) : base(GetMessageText(key), inner) { }

        #endregion


        #region Supporting Methods

        private static String GetMessageText(TKey key) => $"Failed to delete '{nameof(TRecord)}' with key '{key}'.";

        #endregion
    }
}