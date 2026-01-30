using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
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
        var arguments = new RenderArgs<SurLaTableRecord>(serializer, registry);
        var query = definition?.Render(arguments);

        Assert.Multiple(() =>
        {
            if (query != null)
            {
                Assert.That(query, Is.Not.Null);
                Assert.That(query.ElementCount, Is.EqualTo(1));
                Assert.That(query.Elements.First().Name, Is.EqualTo("_id"));
                Assert.That(query.ToString(), Is.EqualTo("{ \"_id\" : 123 }"));
            }
            else
            {
                throw new NullReferenceException();
            }
        });
    }

    [Test]
    public void FilterCriteria_GreaterThan_IsValid()
    {
        var criteria = new[] { new FilterCriteria("Id", FilterOperation.GreaterThan, 123) };
        var definition = criteria.ToFilterDefinition<SurLaTableRecord>();
        var registry = BsonSerializer.SerializerRegistry;
        var serializer = registry.GetSerializer<SurLaTableRecord>();
        var arguments = new RenderArgs<SurLaTableRecord>(serializer, registry);
        var query = definition?.Render(arguments);

        Assert.Multiple(() =>
        {
            if (query != null)
            {
                Assert.That(query.ElementCount, Is.EqualTo(1));
                Assert.That(query.Elements.First().Name, Is.EqualTo("_id"));
                Assert.That(query.ToString(), Is.EqualTo("{ \"_id\" : { \"$gt\" : 123 } }"));
            }
            else
            {
                throw new NullReferenceException();
            }
        });
    }

    [Test]
    public void FilterCriteria_GreaterThanOrEqual_IsValid()
    {
        var criteria = new[] { new FilterCriteria("Id", FilterOperation.GreaterThanOrEqual, 123) };
        var definition = criteria.ToFilterDefinition<SurLaTableRecord>();
        var registry = BsonSerializer.SerializerRegistry;
        var serializer = registry.GetSerializer<SurLaTableRecord>();
        var arguments = new RenderArgs<SurLaTableRecord>(serializer, registry);
        var query = definition?.Render(arguments);

        Assert.Multiple(() =>
        {
            if (query != null)
            {
                Assert.That(query.ElementCount, Is.EqualTo(1));
                Assert.That(query.Elements.First().Name, Is.EqualTo("_id"));
                Assert.That(query.ToString(), Is.EqualTo("{ \"_id\" : { \"$gte\" : 123 } }"));
            }
            else
            {
                throw new NullReferenceException();
            }
        });
    }

    [Test]
    public void FilterCriteria_LessThan_IsValid()
    {
        var criteria = new[] { new FilterCriteria("Id", FilterOperation.LessThan, 123) };
        var definition = criteria.ToFilterDefinition<SurLaTableRecord>();
        var registry = BsonSerializer.SerializerRegistry;
        var serializer = registry.GetSerializer<SurLaTableRecord>();
        var arguments = new RenderArgs<SurLaTableRecord>(serializer, registry);
        var query = definition?.Render(arguments);

        Assert.Multiple(() =>
        {
            if (query != null)
            {
                Assert.That(query.ElementCount, Is.EqualTo(1));
                Assert.That(query.Elements.First().Name, Is.EqualTo("_id"));
                Assert.That(query.ToString(), Is.EqualTo("{ \"_id\" : { \"$lt\" : 123 } }"));
            }
            else
            {
                throw new NullReferenceException();
            }
        });
    }

    [Test]
    public void FilterCriteria_LessThanOrEqual_IsValid()
    {
        var criteria = new[] { new FilterCriteria("Id", FilterOperation.LessThanOrEqual, 123) };
        var definition = criteria.ToFilterDefinition<SurLaTableRecord>();
        var registry = BsonSerializer.SerializerRegistry;
        var serializer = registry.GetSerializer<SurLaTableRecord>();
        var arguments = new RenderArgs<SurLaTableRecord>(serializer, registry);
        var query = definition?.Render(arguments);

        Assert.Multiple(() =>
        {
            if (query != null)
            {
                Assert.That(query.ElementCount, Is.EqualTo(1));
                Assert.That(query.Elements.First().Name, Is.EqualTo("_id"));
                Assert.That(query.ToString(), Is.EqualTo("{ \"_id\" : { \"$lte\" : 123 } }"));
            }
            else
            {
                throw new NullReferenceException();
            }
        });
    }

    [Test]
    public void FilterCriteria_NotEqual_IsValid()
    {
        var criteria = new[] { new FilterCriteria("Id", FilterOperation.NotEqual, 123) };
        var definition = criteria.ToFilterDefinition<SurLaTableRecord>();
        var registry = BsonSerializer.SerializerRegistry;
        var serializer = registry.GetSerializer<SurLaTableRecord>();
        var arguments = new RenderArgs<SurLaTableRecord>(serializer, registry);
        var query = definition?.Render(arguments);

        Assert.Multiple(() =>
        {
            if (query != null)
            {
                Assert.That(query.ElementCount, Is.EqualTo(1));
                Assert.That(query.Elements.First().Name, Is.EqualTo("_id"));
                Assert.That(query.ToString(), Is.EqualTo("{ \"_id\" : { \"$ne\" : 123 } }"));
            }
            else
            {
                throw new NullReferenceException();
            }
        });
    }

    [Test]
    public void FilterCriteria_In_IsValid()
    {
        var criteria = new[] { new FilterCriteria("Id", FilterOperation.In, new[] { 123, 456 }) };
        var definition = criteria.ToFilterDefinition<SurLaTableRecord>();
        var registry = BsonSerializer.SerializerRegistry;
        var serializer = registry.GetSerializer<SurLaTableRecord>();
        var arguments = new RenderArgs<SurLaTableRecord>(serializer, registry);
        var query = definition?.Render(arguments);

        Assert.Multiple(() =>
        {
            if (query != null)
            {
                Assert.That(query.ElementCount, Is.EqualTo(1));
                Assert.That(query.Elements.First().Name, Is.EqualTo("_id"));
                Assert.That(query.ToString(), Is.EqualTo("{ \"_id\" : { \"$in\" : [123, 456] } }"));
            }
            else
            {
                throw new NullReferenceException();
            }
        });
    }

    [Test]
    public void FilterCriteria_In_WithInvalidParameter_IsNotValid()
    {
        var criteria = new[] { new FilterCriteria("Id", FilterOperation.In, 123) };

        Assert.Throws<ArgumentException>(() => criteria.ToFilterDefinition<SurLaTableRecord>());
    }

    public class SurLaTableRecord
    {
        [BsonId] public Int32 Id { get; set; }
        public String Name { get; set; } = String.Empty;
    }
}
