using System;

namespace YuckQi.Data.Exceptions
{
    public sealed class RecordUpdateException<TRecord, TKey> : ApplicationException where TKey : struct
    {
        #region Constructors

        public RecordUpdateException(TKey key) : base(GetMessageText(key))
        {
        }

        public RecordUpdateException(TKey key, Exception inner) : base(GetMessageText(key), inner)
        {
        }

        #endregion


        #region Supporting Methods

        private static string GetMessageText(TKey key)
        {
            return $"Failed to update '{nameof(TRecord)}' with key '{key}'.";
        }

        #endregion
    }
}