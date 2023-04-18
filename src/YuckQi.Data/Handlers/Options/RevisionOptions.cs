namespace YuckQi.Data.Handlers.Options;

public class RevisionOptions
{
    public PropertyHandling RevisionMomentAssignment { get; }

    public RevisionOptions(PropertyHandling revisionMomentAssignment = PropertyHandling.Manual)
    {
        RevisionMomentAssignment = revisionMomentAssignment;
    }
}
