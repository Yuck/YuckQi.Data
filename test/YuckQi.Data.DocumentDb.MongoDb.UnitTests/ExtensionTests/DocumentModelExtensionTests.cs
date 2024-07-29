using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using NUnit.Framework;
using YuckQi.Data.DocumentDb.MongoDb.Attributes;
using YuckQi.Data.DocumentDb.MongoDb.Extensions;

namespace YuckQi.Data.DocumentDb.MongoDb.UnitTests.ExtensionTests;

public class DocumentModelExtensionTests
{
    private static readonly SurLaTableDocument? NullSurLaTableRecord = null;
    private static readonly Type? NullType = null;

    [SetUp]
    public void Setup() { }

    [Test]
    public void GetCollectionName_WithNullType_IsNull()
    {
        var name = NullType.GetCollectionName();

        Assert.That(name, Is.Null);
    }

    [Test]
    public async Task GetCollectionName_WithMultipleRequests_IsValid()
    {
        var type = typeof(SurLaTableDocument);
        var tasks = new[] { 1, 2, 3, 4, 5 }.Select(_ => Task.Run(() => type.GetCollectionName())).ToList();

        await Task.WhenAll(tasks);

        var first = tasks.First().Result;

        Assert.That(tasks.All(t => Equals(t.Result, first)), Is.True);
    }

    [Test]
    public void GetDatabaseName_WithNullType_IsNull()
    {
        var name = NullType.GetDatabaseName();

        Assert.That(name, Is.Null);
    }

    [Test]
    public async Task GetDatabaseName_WithMultipleRequests_IsValid()
    {
        var type = typeof(SurLaTableDocument);
        var tasks = new[] { 1, 2, 3, 4, 5 }.Select(_ => Task.Run(() => type.GetDatabaseName())).ToList();

        await Task.WhenAll(tasks);

        var first = tasks.First().Result;

        Assert.That(tasks.All(t => Equals(t.Result, first)), Is.True);
    }

    [Test]
    public void GetIdentifierFieldDefinition_WithNullType_IsNull()
    {
        var name = NullType.GetIdentifierFieldDefinition<SurLaTableDocument, Int32>();

        Assert.That(name, Is.Null);
    }

    [Test]
    public async Task GetIdentifierFieldDefinition_WithMultipleRequests_IsValid()
    {
        var type = typeof(SurLaTableDocument);
        var tasks = new[] { 1, 2, 3, 4, 5 }.Select(_ => Task.Run(() => type.GetIdentifierFieldDefinition<SurLaTableDocument, Int32>())).ToList();

        await Task.WhenAll(tasks);

        var registry = BsonSerializer.SerializerRegistry;
        var serializer = registry.GetSerializer<SurLaTableDocument>();
        var first = tasks.First().Result;
        var field = first?.Render(serializer, registry).FieldName;

        Assert.That(tasks.All(t => Equals(t.Result?.Render(serializer, registry).FieldName, field)), Is.True);
    }

    [Test]
    public void GetIdentifier_WithNullRecord_IsNull()
    {
        var name = NullSurLaTableRecord?.GetIdentifier<SurLaTableDocument, Int32>();

        Assert.That(name, Is.Null);
    }

    [Test]
    public async Task GetIdentifier_WithMultipleRequests_IsValid()
    {
        var document = new SurLaTableDocument { Id = 1, Name = "test" };
        var tasks = new[] { 1, 2, 3, 4, 5 }.Select(_ => Task.Run(() => document.GetIdentifier<SurLaTableDocument, Int32>())).ToList();

        await Task.WhenAll(tasks);

        var first = tasks.First().Result;

        Assert.That(tasks.All(t => Equals(t.Result, first)), Is.True);
    }

    [Database("Tableau")]
    public class SurLaTableDocument
    {
        [BsonId] public Int32 Id { get; set; }
        public String Name { get; set; } = String.Empty;
    }
}
