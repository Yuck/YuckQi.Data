using System.Collections.Concurrent;
using NUnit.Framework;
using YuckQi.Data.Exceptions;
using YuckQi.Data.MemDb.Handlers;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.MemDb.UnitTests.Handlers;

public class PhysicalDeletionHandlerTests
{
    [SetUp]
    public void Setup() { }

    [Test]
    public void A()
    {
        var entities = new ConcurrentDictionary<Int32, SurLaTable>();
        var creator = new CreationHandler<SurLaTable, Int32, Object>(entities);
        var deleter = new PhysicalDeletionHandler<SurLaTable, Int32, Object>(entities);
        var scope = new Object();
        var entity = new SurLaTable { Identifier = 1, Name = "ABC" };
        var created = creator.Create(entity, scope);

        Assert.That(entities.Values.ToList(), Does.Contain(created));

        var deleted = deleter.Delete(created, scope);

        Assert.That(entities.Values.ToList(), Does.Not.Contain(deleted));
    }

    [Test]
    public void B()
    {
        var entities = new ConcurrentDictionary<Int32, SurLaTable>();
        var deleter = new PhysicalDeletionHandler<SurLaTable, Int32, Object>(entities);
        var scope = new Object();
        var entity = new SurLaTable { Identifier = 1, Name = "ABC" };

        Assert.Throws<PhysicalDeletionException<SurLaTable, Int32>>(() => deleter.Delete(entity, scope));
    }

    public class SurLaTable : EntityBase<Int32>, ICreated
    {
        public String Name { get; set; } = String.Empty;

        public DateTime CreationMomentUtc { get; set; }
    }
}
