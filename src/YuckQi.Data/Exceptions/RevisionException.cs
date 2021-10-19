using System;

namespace YuckQi.Data.Exceptions
{
    public sealed class RevisionException<TRecord, TKey> : ApplicationException where TKey : struct
    {
        #region Constructors

        public RevisionException(TKey key) : base(GetMessageText(key)) { }

        public RevisionException(TKey key, Exception inner) : base(GetMessageText(key), inner) { }

        #endregion


        #region Supporting Methods

        private static String GetMessageText(TKey key) => $"Failed to revise '{nameof(TRecord)}' with key '{key}'.";

        #endregion
    }
}
