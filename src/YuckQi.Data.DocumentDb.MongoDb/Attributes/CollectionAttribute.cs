namespace YuckQi.Data.DocumentDb.MongoDb.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class CollectionAttribute : Attribute
{
    public String Name { get; }

    public CollectionAttribute(String name)
    {
        Name = name;
    }
}
