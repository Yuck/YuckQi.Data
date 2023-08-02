namespace YuckQi.Data.Exceptions;

public sealed class PhysicalDeletionException<TEntity, TIdentifier> : ApplicationException
{
    public PhysicalDeletionException(TIdentifier? identifier) : base(GetMessageText(identifier)) { }

    public PhysicalDeletionException(TIdentifier? identifier, Exception inner) : base(GetMessageText(identifier), inner) { }

    private static String GetMessageText(TIdentifier? identifier) => $"Failed to delete '{typeof(TEntity).Name}' with identifier '{identifier}'.";
}
