using System.Collections.Concurrent;
using NUnit.Framework;
using YuckQi.Data.MemDb.Handlers;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.MemDb.UnitTests.Handlers;

public class RetrievalHandlerTests
{
    [SetUp]
    public void Setup() { }

    [Test]
    public void A()
    {
        var entities = new ConcurrentDictionary<Int32, SurLaTable>();
        var creator = new CreationHandler<SurLaTable, Int32, Object>(entities);
        var retriever = new RetrievalHandler<SurLaTable, Int32, Object>(entities);
        var scope = new Object();
        var entity = new SurLaTable { Identifier = 1, Name = "ABC" };

        var created = creator.Create(entity, scope);

        Assert.That(entities.Values.ToList(), Does.Contain(created));

        var retrieved = retriever.Get(created.Identifier, scope);

        Assert.That(retrieved.Identifier, Is.EqualTo(created.Identifier));
    }

    [Test]
    public void B()
    {
        var entities = new ConcurrentDictionary<Int32, SurLaTable>();
        var creator = new CreationHandler<SurLaTable, Int32, Object>(entities);
        var retriever = new RetrievalHandler<SurLaTable, Int32, Object>(entities);
        var scope = new Object();
        var entity = new SurLaTable { Identifier = 1, Name = "ABC" };

        var created = creator.Create(entity, scope);

        Assert.That(entities.Values.ToList(), Does.Contain(created));

        var parameters = new { Name = "ABC" };
        var retrieved = retriever.Get(parameters, scope);

        Assert.That(retrieved.Identifier, Is.EqualTo(created.Identifier));
    }

    [Test]
    public void C()
    {
        var entities = new ConcurrentDictionary<Int32, SurLaTable>();
        var creator = new CreationHandler<SurLaTable, Int32, Object>(entities);
        var retriever = new RetrievalHandler<SurLaTable, Int32, Object>(entities);
        var scope = new Object();
        var entity = new SurLaTable { Identifier = 1, Name = "ABC" };

        var created = creator.Create(entity, scope);

        Assert.That(entities.Values.ToList(), Does.Contain(created));

        var parameters = new { Name = "ZZZ" };
        var retrieved = retriever.Get(parameters, scope);

        Assert.That(retrieved, Is.Null);
    }

    public class SurLaTable : EntityBase<Int32>, ICreated
    {
        public String Name { get; set; }

        public DateTime CreationMomentUtc { get; set; }
    }
}
