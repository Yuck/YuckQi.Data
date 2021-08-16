using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using NUnit.Framework;
using YuckQi.Data.DocumentDb.MongoDb.Extensions;

namespace YuckQi.Data.DocumentDb.MongoDb.UnitTests.ExtensionTests
{
    public class DocumentModelExtensionTests
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void GetCollectionName_WithNullType_IsNull()
        {
            var name = ((Type) null).GetCollectionName();

            Assert.IsNull(name);
        }

        [Test]
        public async Task GetCollectionName_WithMultipleRequests_IsValid()
        {
            var type = typeof(SurLaTableRecord);
            var tasks = new[] { 1, 2, 3, 4, 5 }.Select(_ => Task.Run(() => type.GetCollectionName())).ToList();
            var result = await Task.WhenAll(tasks);
            var first = tasks.First().Result;

            Assert.IsTrue(tasks.All(t => Equals(t.Result, first)));
        }

        [Test]
        public void GetDatabaseName_WithNullType_IsNull()
        {
            var name = ((Type) null).GetDatabaseName();

            Assert.IsNull(name);
        }

        [Test]
        public async Task GetDatabaseName_WithMultipleRequests_IsValid()
        {
            var type = typeof(SurLaTableRecord);
            var tasks = new[] { 1, 2, 3, 4, 5 }.Select(_ => Task.Run(() => type.GetDatabaseName())).ToList();
            var result = await Task.WhenAll(tasks);
            var first = tasks.First().Result;

            Assert.IsTrue(tasks.All(t => Equals(t.Result, first)));
        }

        [Test]
        public void GetKeyFieldDefinition_WithNullType_IsNull()
        {
            var name = ((Type) null).GetKeyFieldDefinition<SurLaTableRecord, Int32>();

            Assert.IsNull(name);
        }

        [Test]
        public async Task GetKeyFieldDefinition_WithMultipleRequests_IsValid()
        {
            var type = typeof(SurLaTableRecord);
            var tasks = new[] { 1, 2, 3, 4, 5 }.Select(_ => Task.Run(() => type.GetKeyFieldDefinition<SurLaTableRecord, Int32>())).ToList();
            var result = await Task.WhenAll(tasks);
            var registry = BsonSerializer.SerializerRegistry;
            var serializer = registry.GetSerializer<SurLaTableRecord>();
            var first = tasks.First().Result.Render(serializer, registry).FieldName;

            Assert.IsTrue(tasks.All(t => Equals(t.Result.Render(serializer, registry).FieldName, first)));
        }

        [Test]
        public void GetKey_WithNullRecord_IsNull()
        {
            var name = ((SurLaTableRecord) null).GetKey<SurLaTableRecord, Int32>();

            Assert.IsNull(name);
        }

        [Test]
        public async Task GetKey_WithMultipleRequests_IsValid()
        {
            var record = new SurLaTableRecord { Id = 1, Name = "test" };
            var tasks = new[] { 1, 2, 3, 4, 5 }.Select(_ => Task.Run(() => record.GetKey<SurLaTableRecord, Int32>())).ToList();
            var result = await Task.WhenAll(tasks);
            var first = tasks.First().Result;

            Assert.IsTrue(tasks.All(t => Equals(t.Result, first)));
        }
    }
}
