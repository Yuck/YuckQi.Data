using System.Collections.Concurrent;
using NUnit.Framework;
using YuckQi.Data.Exceptions;
using YuckQi.Data.MemDb.Handlers;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.MemDb.UnitTests.Handlers;

public class CreationHandlerTests
{
    [SetUp]
    public void Setup() { }

    [Test]
    public void A()
    {
        var entities = new ConcurrentDictionary<Int32, SurLaTable>();
        var creator = new CreationHandler<SurLaTable, Int32, Object>(entities);
        var scope = new Object();
        var entity = new SurLaTable { Identifier = 1, Name = "ABC" };

        var created = creator.Create(entity, scope);

        Assert.Multiple(() =>
        {
            Assert.That(entities.Values.ToList(), Does.Contain(created));
            Assert.That(entity.Identifier, Is.EqualTo(created.Identifier));
        });
    }

    [Test]
    public void B()
    {
        var entities = new ConcurrentDictionary<Int32, SurLaTable>();
        var creator = new CreationHandler<SurLaTable, Int32, Object>(entities);
        var scope = new Object();
        var entity = new SurLaTable { Identifier = 1, Name = "ABC" };

        var _ = creator.Create(entity, scope);

        Assert.That(() => creator.Create(entity, scope), Throws.Exception.TypeOf<CreationException<SurLaTable>>());
    }

    public class SurLaTable : EntityBase<Int32>, ICreated
    {
        public String Name { get; set; } = String.Empty;

        public DateTime CreationMomentUtc { get; set; }
    }
}
