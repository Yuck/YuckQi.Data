namespace YuckQi.Data.Exceptions;

public sealed class RevisionException<TEntity, TIdentifier> : ApplicationException
{
    public RevisionException(TIdentifier? identifier) : base(GetMessageText(identifier)) { }

    public RevisionException(TIdentifier? identifier, Exception inner) : base(GetMessageText(identifier), inner) { }

    private static String GetMessageText(TIdentifier? identifier) => $"Failed to revise '{typeof(TEntity).Name}' with identifier '{identifier}'.";
}
