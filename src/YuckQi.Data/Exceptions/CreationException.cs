namespace YuckQi.Data.Exceptions;

public sealed class CreationException<TEntity> : ApplicationException
{
    public CreationException() : base(GetMessageText()) { }

    public CreationException(Exception inner) : base(GetMessageText(), inner) { }

    private static String GetMessageText() => $"Failed to create '{typeof(TEntity).Name}'.";
}
