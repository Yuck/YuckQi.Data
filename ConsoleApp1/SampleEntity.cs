using Amazon.DynamoDBv2.DataModel;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace ConsoleApp1;

public class SampleEntity : EntityBase<SampleEntityKey>, IActivated, ICreated, IDeleted, IRevised
{
    public DateTime? ActivationMomentUtc { get; set; }
    public DateTime? DeletionMomentUtc { get; set; }
    public String Name { get; set; } = "N/A";
    public DateTime CreationMomentUtc { get; set; }
    public DateTime RevisionMomentUtc { get; set; }
}

public class SampleEntityKey : IEquatable<SampleEntityKey>
{
    public Guid Id { get; init; }

    public Boolean Equals(SampleEntityKey? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id);
    }

    public override Boolean Equals(Object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((SampleEntityKey) obj);
    }

    public override Int32 GetHashCode() => Id.GetHashCode();
}

[DynamoDBTable("sample-entity-view")]
public class SampleEntityRecord
{
    [DynamoDBHashKey("PK")] public String PartitionKey { get; set; }
    [DynamoDBRangeKey("SK")] public String SortKey { get; set; }
    public String Name { get; set; }
    public DateTime? ActivationMomentUtc { get; set; }
    public DateTime CreationMomentUtc { get; set; }
    public DateTime? DeletionMomentUtc { get; set; }
    public DateTime RevisionMomentUtc { get; set; }
}
