using System.Collections.Concurrent;
using NUnit.Framework;
using YuckQi.Data.Handlers.Options;
using YuckQi.Data.MemDb.Handlers;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.MemDb.UnitTests.Handlers;

public class LogicalDeletionHandlerTests
{
    [SetUp]
    public void Setup() { }

    [Test]
    public void A()
    {
        var entities = new ConcurrentDictionary<Int32, SurLaTable>();
        var creator = new CreationHandler<SurLaTable, Int32, Object>(entities, new CreationOptions<Int32>(creationMomentAssignment: PropertyHandling.Auto));
        var reviser = new RevisionHandler<SurLaTable, Int32, Object>(entities, new RevisionOptions(PropertyHandling.Auto));
        var deleter = new LogicalDeletionHandler<SurLaTable, Int32, Object>(reviser);
        var scope = new Object();
        var entity = new SurLaTable { Identifier = 1, Name = "ABC" };
        var created = creator.Create(entity, scope);

        Assert.That(created.DeletionMomentUtc, Is.Null);

        var deleted = deleter.Delete(created, scope);

        Assert.That(deleted.DeletionMomentUtc, Is.Not.Null);
    }

    [Test]
    public void B()
    {
        var entities = new ConcurrentDictionary<Int32, SurLaTable>();
        var creator = new CreationHandler<SurLaTable, Int32, Object>(entities, new CreationOptions<Int32>(creationMomentAssignment: PropertyHandling.Auto));
        var reviser = new RevisionHandler<SurLaTable, Int32, Object>(entities, new RevisionOptions(PropertyHandling.Auto));
        var deleter = new LogicalDeletionHandler<SurLaTable, Int32, Object>(reviser);
        var scope = new Object();
        var entity = new SurLaTable { Identifier = 1, Name = "ABC" };
        var created = creator.Create(entity, scope);

        Assert.That(created.DeletionMomentUtc, Is.Null);

        var deleted = deleter.Delete(created, scope);

        Assert.That(deleted.DeletionMomentUtc, Is.Not.Null);

        var deletionMomentUtc = deleted.DeletionMomentUtc;
        var revisionMomentUtc = deleted.RevisionMomentUtc;
        var _ = deleter.Delete(created, scope);

        Assert.Multiple(() =>
        {
            Assert.That(deleted.DeletionMomentUtc, Is.EqualTo(deletionMomentUtc));
            Assert.That(deleted.RevisionMomentUtc, Is.EqualTo(revisionMomentUtc));
        });
    }

    public class SurLaTable : EntityBase<Int32>, ICreated, IDeleted, IRevised
    {
        public String Name { get; set; } = String.Empty;

        public DateTime CreationMomentUtc { get; set; }
        public DateTime? DeletionMomentUtc { get; set; }
        public DateTime RevisionMomentUtc { get; set; }
    }
}
