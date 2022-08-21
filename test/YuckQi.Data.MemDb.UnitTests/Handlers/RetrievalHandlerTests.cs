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
        var retrieved = retriever.Get(created.Identifier, scope);

        Assert.Multiple(() =>
        {
            Assert.That(entities.Values.ToList(), Does.Contain(created));
            Assert.That(retrieved?.Identifier, Is.EqualTo(created.Identifier));
        });
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
        var parameters = new { Name = "ABC" };
        var retrieved = retriever.Get(parameters, scope);

        Assert.Multiple(() =>
        {
            Assert.That(entities.Values.ToList(), Does.Contain(created));
            Assert.That(retrieved?.Identifier, Is.EqualTo(created.Identifier));
        });
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
        var parameters = new { Name = "ZZZ" };
        var retrieved = retriever.Get(parameters, scope);

        Assert.Multiple(() =>
        {
            Assert.That(entities.Values.ToList(), Does.Contain(created));
            Assert.That(retrieved, Is.Null);
        });
    }

    [Test]
    public void D()
    {
        var entities = new ConcurrentDictionary<Int32, SurLaTable>();
        var creator = new CreationHandler<SurLaTable, Int32, Object>(entities);
        var retriever = new RetrievalHandler<SurLaTable, Int32, Object>(entities);
        var scope = new Object();
        var entity = new SurLaTable { Identifier = 1, Name = "ABC" };

        var created = creator.Create(entity, scope);
        var parameters = new { Name = "ABC" };
        var retrieved = retriever.GetList(parameters, scope);

        Assert.Multiple(() =>
        {
            Assert.That(entities.Values.ToList(), Does.Contain(created));
            Assert.That(retrieved, Contains.Item(created));
        });
    }

    [Test]
    public void E()
    {
        var entities = new ConcurrentDictionary<Int32, SurLaTable>();
        var creator = new CreationHandler<SurLaTable, Int32, Object>(entities);
        var retriever = new RetrievalHandler<SurLaTable, Int32, Object>(entities);
        var scope = new Object();
        var entity = new SurLaTable { Identifier = 1, Name = "ABC" };

        var created = creator.Create(entity, scope);
        var parameters = new { Name = "ZZZ" };
        var retrieved = retriever.GetList(parameters, scope);

        Assert.Multiple(() =>
        {
            Assert.That(entities.Values.ToList(), Does.Contain(created));
            Assert.That(retrieved, Does.Not.Contain(created));
        });
    }

    public class SurLaTable : EntityBase<Int32>, ICreated
    {
        public String Name { get; set; } = String.Empty;

        public DateTime CreationMomentUtc { get; set; }
    }
}
