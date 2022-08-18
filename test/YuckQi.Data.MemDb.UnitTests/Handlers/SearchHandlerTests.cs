using System.Collections.Concurrent;
using NUnit.Framework;
using YuckQi.Data.MemDb.Handlers;
using YuckQi.Data.Sorting;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Domain.ValueObjects;

namespace YuckQi.Data.MemDb.UnitTests.Handlers;

public class SearchHandlerTests
{
    [SetUp]
    public void Setup() { }

    [Test]
    public void A()
    {
        var entities = new ConcurrentDictionary<Int32, SurLaTable>();
        var creator = new CreationHandler<SurLaTable, Int32, Object>(entities);
        var searcher = new SearchHandler<SurLaTable, Int32, Object>(entities);
        var scope = new Object();
        var entity = new SurLaTable { Identifier = 1, Name = "ABC" };

        var created = creator.Create(entity, scope);

        Assert.That(entities.Values.ToList(), Does.Contain(created));

        var parameters = new { Identifier = 1 };
        var page = new Page(1, 10);
        var sort = new[] { new SortCriteria("Identifier", SortOrder.Descending) }.OrderBy(_ => 1);
        var found = searcher.Search(parameters, page, sort, scope);

        Assert.That(found.Items, Contains.Item(created));
    }

    [Test]
    public void B()
    {
        var entities = new ConcurrentDictionary<Int32, SurLaTable>();
        var creator = new CreationHandler<SurLaTable, Int32, Object>(entities);
        var searcher = new SearchHandler<SurLaTable, Int32, Object>(entities);
        var scope = new Object();
        var entity = new SurLaTable { Identifier = 1, Name = "ABC" };

        var created = creator.Create(entity, scope);

        Assert.That(entities.Values.ToList(), Does.Contain(created));

        var parameters = new { Name = "ABC" };
        var page = new Page(1, 10);
        var sort = new[] { new SortCriteria("Identifier", SortOrder.Descending) }.OrderBy(_ => 1);
        var found = searcher.Search(parameters, page, sort, scope);

        Assert.That(found.Items, Contains.Item(created));
    }

    [Test]
    public void C()
    {
        var entities = new ConcurrentDictionary<Int32, SurLaTable>();
        var creator = new CreationHandler<SurLaTable, Int32, Object>(entities);
        var searcher = new SearchHandler<SurLaTable, Int32, Object>(entities);
        var scope = new Object();
        var entity = new SurLaTable { Identifier = 1, Name = "ABC" };

        var created = creator.Create(entity, scope);

        Assert.That(entities.Values.ToList(), Does.Contain(created));

        var parameters = new { Name = "ZZZ" };
        var page = new Page(1, 10);
        var sort = new[] { new SortCriteria("Identifier", SortOrder.Descending) }.OrderBy(_ => 1);
        var found = searcher.Search(parameters, page, sort, scope);

        Assert.That(found.Items, Does.Not.Contain(created));
    }

    public class SurLaTable : EntityBase<Int32>, ICreated
    {
        public String Name { get; set; }

        public DateTime CreationMomentUtc { get; set; }
    }
}
