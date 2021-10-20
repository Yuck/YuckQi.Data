namespace YuckQi.Data.Handlers.Options
{
    public class CreationOptions
    {
        public PropertyHandling CreationMomentAssignment { get; }
        public PropertyHandling RevisionMomentAssignment { get; }

        public CreationOptions(PropertyHandling creationMomentAssignment = PropertyHandling.Manual, PropertyHandling revisionMomentAssignment = PropertyHandling.Manual)
        {
            CreationMomentAssignment = creationMomentAssignment;
            RevisionMomentAssignment = revisionMomentAssignment;
        }
    }
}
