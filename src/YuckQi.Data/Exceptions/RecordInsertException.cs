using System;

namespace YuckQi.Data.Exceptions
{
    public sealed class RecordInsertException<TRecord> : ApplicationException
    {
        #region Constructors

        public RecordInsertException() : base(GetMessageText())
        {
        }

        public RecordInsertException(Exception inner) : base(GetMessageText(), inner)
        {
        }

        #endregion


        #region Supporting Methods

        private static string GetMessageText()
        {
            return $"Failed to insert '{nameof(TRecord)}'.";
        }

        #endregion
    }
}