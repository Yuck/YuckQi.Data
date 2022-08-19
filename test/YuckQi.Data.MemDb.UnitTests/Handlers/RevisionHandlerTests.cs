using System.Collections.Concurrent;
using NUnit.Framework;
using YuckQi.Data.Exceptions;
using YuckQi.Data.Handlers.Options;
using YuckQi.Data.MemDb.Handlers;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.MemDb.UnitTests.Handlers;

public class RevisionHandlerTests
{
    [SetUp]
    public void Setup() { }

    [Test]
    public void A()
    {
        var entities = new ConcurrentDictionary<Int32, SurLaTable>(new Dictionary<Int32, SurLaTable> { { 1, new SurLaTable { Identifier = 1, Name = "ABC" } } });
        var reviser = new RevisionHandler<SurLaTable, Int32, Object>(entities, new RevisionOptions(PropertyHandling.Auto));
        var scope = new Object();
        var entity = new SurLaTable { Identifier = 1, Name = "ABC" };

        var revised = reviser.Revise(entity, scope);

        Assert.Multiple(() =>
        {
            Assert.That(entity.Identifier, Is.EqualTo(revised.Identifier));
            Assert.That(entity.RevisionMomentUtc, Is.GreaterThan(DateTime.MinValue));
            Assert.That(entities.Values.ToList(), Does.Contain(revised));
        });
    }

    [Test]
    public void B()
    {
        var entities = new ConcurrentDictionary<Int32, SurLaTable>(new Dictionary<Int32, SurLaTable> { { 1, new SurLaTable { Identifier = 1, Name = "ABC" } } });
        var reviser = new RevisionHandler<SurLaTable, Int32, Object>(entities);
        var scope = new Object();
        var revisionMomentUtc = DateTime.UtcNow;
        var entity = new SurLaTable { Identifier = 1, Name = "ABC", RevisionMomentUtc = revisionMomentUtc };

        var revised = reviser.Revise(entity, scope);

        Assert.Multiple(() =>
        {
            Assert.That(entity.Identifier, Is.EqualTo(revised.Identifier));
            Assert.That(entity.RevisionMomentUtc, Is.EqualTo(revisionMomentUtc));
            Assert.That(entities.Values.ToList(), Does.Contain(revised));
        });
    }

    [Test]
    public void C()
    {
        var entities = new ConcurrentDictionary<Int32, SurLaTable>(new Dictionary<Int32, SurLaTable> { { 1, new SurLaTable { Identifier = 1, Name = "ABC" } } });
        var reviser = new RevisionHandler<SurLaTable, Int32, Object>(entities, new RevisionOptions(PropertyHandling.Auto));
        var scope = new Object();
        var entity = new SurLaTable { Identifier = 2, Name = "ABC" };

        Assert.That(() => reviser.Revise(entity, scope), Throws.TypeOf<RevisionException<SurLaTable, Int32>>());
    }

    public class SurLaTable : EntityBase<Int32>, IRevised
    {
        public String Name { get; set; } = String.Empty;

        public DateTime RevisionMomentUtc { get; set; }
    }
}
