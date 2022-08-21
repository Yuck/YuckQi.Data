namespace YuckQi.Data.Handlers.Options;

public class CreationOptions<TIdentifier>
{
    public PropertyHandling CreationMomentAssignment { get; }
    public Func<TIdentifier>? IdentifierFactory { get; }
    public PropertyHandling RevisionMomentAssignment { get; }

    public CreationOptions(Func<TIdentifier>? identifierFactory = null, PropertyHandling creationMomentAssignment = PropertyHandling.Manual, PropertyHandling revisionMomentAssignment = PropertyHandling.Manual)
    {
        IdentifierFactory = identifierFactory;
        CreationMomentAssignment = creationMomentAssignment;
        RevisionMomentAssignment = revisionMomentAssignment;
    }
}
