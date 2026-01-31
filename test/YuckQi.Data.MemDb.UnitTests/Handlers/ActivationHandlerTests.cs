using System.Collections.Concurrent;
using NUnit.Framework;
using YuckQi.Data.MemDb.Handlers.Write;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.MemDb.UnitTests.Handlers;

public class ActivationHandlerTests
{
    [SetUp]
    public void Setup() { }

    [Test]
    public void A()
    {
        var entities = new ConcurrentDictionary<Int32, SurLaTable>();
        var creator = new CreationHandler<SurLaTable, Int32, Object>(entities);
        var reviser = new RevisionHandler<SurLaTable, Int32, Object>(entities);
        var activator = new ActivationHandler<SurLaTable, Int32, Object>(reviser);
        var scope = new Object();
        var entity = new SurLaTable { Identifier = 1, Name = "ABC" };

        var created = creator.Create(entity, scope);

        Assert.That(created.ActivationMomentUtc, Is.Null);

        var activated = activator.Activate(created, scope);

        Assert.That(activated.ActivationMomentUtc, Is.Not.Null);
    }

    [Test]
    public void B()
    {
        var entities = new ConcurrentDictionary<Int32, SurLaTable>();
        var creator = new CreationHandler<SurLaTable, Int32, Object>(entities);
        var reviser = new RevisionHandler<SurLaTable, Int32, Object>(entities);
        var activator = new ActivationHandler<SurLaTable, Int32, Object>(reviser);
        var scope = new Object();
        var entity = new SurLaTable { Identifier = 1, Name = "ABC", ActivationMomentUtc = DateTime.UtcNow };

        var created = creator.Create(entity, scope);

        Assert.That(created.ActivationMomentUtc, Is.Not.Null);

        var deactivated = activator.Deactivate(created, scope);

        Assert.That(deactivated.ActivationMomentUtc, Is.Null);
    }

    public class SurLaTable : EntityBase<Int32>, IActivated, ICreated, IRevised
    {
        public String Name { get; set; } = String.Empty;

        public DateTime? ActivationMomentUtc { get; set; }
        public DateTime CreationMomentUtc { get; set; }
        public DateTime RevisionMomentUtc { get; set; }
    }
}
