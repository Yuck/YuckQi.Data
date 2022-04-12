using System;
using System.Linq;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using NUnit.Framework;
using YuckQi.Data.DocumentDb.MongoDb.Extensions;
using YuckQi.Data.Filtering;

namespace YuckQi.Data.DocumentDb.MongoDb.UnitTests.ExtensionTests;

public class FilterDefinitionExtensionTests
{
    [SetUp]
    public void Setup() { }

    [Test]
    public void FilterCriteria_Equal_IsValid()
    {
        var criteria = new[] { new FilterCriteria("Id", 123) };
        var definition = criteria.ToFilterDefinition<SurLaTableRecord>();
        var registry = BsonSerializer.SerializerRegistry;
        var serializer = registry.GetSerializer<SurLaTableRecord>();
        var query = definition.Render(serializer, registry);

        Assert.AreEqual(1, query.ElementCount);
        Assert.AreEqual("_id", query.Elements.First().Name);
        Assert.AreEqual("{ \"_id\" : 123 }", query.ToString());
    }

    [Test]
    public void FilterCriteria_GreaterThan_IsValid()
    {
        var criteria = new[] { new FilterCriteria("Id", FilterOperation.GreaterThan, 123) };
        var definition = criteria.ToFilterDefinition<SurLaTableRecord>();
        var registry = BsonSerializer.SerializerRegistry;
        var serializer = registry.GetSerializer<SurLaTableRecord>();
        var query = definition.Render(serializer, registry);

        Assert.AreEqual(1, query.ElementCount);
        Assert.AreEqual("_id", query.Elements.First().Name);
        Assert.AreEqual("{ \"_id\" : { \"$gt\" : 123 } }", query.ToString());
    }

    [Test]
    public void FilterCriteria_GreaterThanOrEqual_IsValid()
    {
        var criteria = new[] { new FilterCriteria("Id", FilterOperation.GreaterThanOrEqual, 123) };
        var definition = criteria.ToFilterDefinition<SurLaTableRecord>();
        var registry = BsonSerializer.SerializerRegistry;
        var serializer = registry.GetSerializer<SurLaTableRecord>();
        var query = definition.Render(serializer, registry);

        Assert.AreEqual(1, query.ElementCount);
        Assert.AreEqual("_id", query.Elements.First().Name);
        Assert.AreEqual("{ \"_id\" : { \"$gte\" : 123 } }", query.ToString());
    }

    [Test]
    public void FilterCriteria_LessThan_IsValid()
    {
        var criteria = new[] { new FilterCriteria("Id", FilterOperation.LessThan, 123) };
        var definition = criteria.ToFilterDefinition<SurLaTableRecord>();
        var registry = BsonSerializer.SerializerRegistry;
        var serializer = registry.GetSerializer<SurLaTableRecord>();
        var query = definition.Render(serializer, registry);

        Assert.AreEqual(1, query.ElementCount);
        Assert.AreEqual("_id", query.Elements.First().Name);
        Assert.AreEqual("{ \"_id\" : { \"$lt\" : 123 } }", query.ToString());
    }

    [Test]
    public void FilterCriteria_LessThanOrEqual_IsValid()
    {
        var criteria = new[] { new FilterCriteria("Id", FilterOperation.LessThanOrEqual, 123) };
        var definition = criteria.ToFilterDefinition<SurLaTableRecord>();
        var registry = BsonSerializer.SerializerRegistry;
        var serializer = registry.GetSerializer<SurLaTableRecord>();
        var query = definition.Render(serializer, registry);

        Assert.AreEqual(1, query.ElementCount);
        Assert.AreEqual("_id", query.Elements.First().Name);
        Assert.AreEqual("{ \"_id\" : { \"$lte\" : 123 } }", query.ToString());
    }

    [Test]
    public void FilterCriteria_NotEqual_IsValid()
    {
        var criteria = new[] { new FilterCriteria("Id", FilterOperation.NotEqual, 123) };
        var definition = criteria.ToFilterDefinition<SurLaTableRecord>();
        var registry = BsonSerializer.SerializerRegistry;
        var serializer = registry.GetSerializer<SurLaTableRecord>();
        var query = definition.Render(serializer, registry);

        Assert.AreEqual(1, query.ElementCount);
        Assert.AreEqual("_id", query.Elements.First().Name);
        Assert.AreEqual("{ \"_id\" : { \"$ne\" : 123 } }", query.ToString());
    }
}

public class SurLaTableRecord
{
    [BsonId] public Int32 Id { get; set; }
    public String Name { get; set; }
}
