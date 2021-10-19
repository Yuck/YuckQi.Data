using System;

namespace YuckQi.Data.Exceptions
{
    public sealed class CreationException<TRecord> : ApplicationException
    {
        #region Constructors

        public CreationException() : base(GetMessageText()) { }

        public CreationException(Exception inner) : base(GetMessageText(), inner) { }

        #endregion


        #region Supporting Methods

        private static String GetMessageText() => $"Failed to create '{nameof(TRecord)}'.";

        #endregion
    }
}
