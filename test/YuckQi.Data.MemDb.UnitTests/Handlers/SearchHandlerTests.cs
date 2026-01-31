using System.Collections.Concurrent;
using System.Text;
using NUnit.Framework;
using YuckQi.Data.Filtering;
using YuckQi.Data.Handlers.Write.Options;
using YuckQi.Data.MemDb.Handlers.Read;
using YuckQi.Data.MemDb.Handlers.Write;
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
        var parameters = new { Identifier = 1 };
        var page = new Page(1, 10);
        var sort = new[] { new SortCriteria("Identifier", SortOrder.Descending) }.OrderBy(_ => 1);
        var found = searcher.Search(parameters, page, sort, scope);

        Assert.Multiple(() =>
        {
            Assert.That(entities.Values.ToList(), Does.Contain(created));
            Assert.That(found.Items, Contains.Item(created));
        });
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
        var parameters = new { Name = "ABC" };
        var page = new Page(1, 10);
        var sort = new[] { new SortCriteria("Identifier", SortOrder.Descending) }.OrderBy(_ => 1);
        var found = searcher.Search(parameters, page, sort, scope);

        Assert.Multiple(() =>
        {
            Assert.That(entities.Values.ToList(), Does.Contain(created));
            Assert.That(found.Items, Contains.Item(created));
        });
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
        var parameters = new { Name = "ZZZ" };
        var page = new Page(1, 10);
        var sort = new[] { new SortCriteria("Identifier", SortOrder.Descending) }.OrderBy(_ => 1);
        var found = searcher.Search(parameters, page, sort, scope);

        Assert.Multiple(() =>
        {
            Assert.That(entities.Values.ToList(), Does.Contain(created));
            Assert.That(found.Items, Does.Not.Contain(created));
        });
    }

    [Test]
    public void D()
    {
        var entities = new ConcurrentDictionary<Int32, SurLaTable>();
        var creator = new CreationHandler<SurLaTable, Int32, Object>(entities, new CreationOptions<Int32>(() => entities.Count + 1));
        var searcher = new SearchHandler<SurLaTable, Int32, Object>(entities);
        var scope = new Object();
        for (var i = 0; i < 50; i++)
            creator.Create(new SurLaTable { Name = "ABC" }, scope);

        var parameters = new[] { new FilterCriteria("Identifier", FilterOperation.LessThanOrEqual, 25) };
        var page = new Page(1, 10);
        var sort = new[] { new SortCriteria("Identifier", SortOrder.Descending) }.OrderBy(_ => 1);
        var found = searcher.Search(parameters, page, sort, scope);

        Assert.Multiple(() =>
        {
            Assert.That(entities.Count, Is.EqualTo(50));
            Assert.That(found.TotalCount, Is.EqualTo(25));
            Assert.That(found.Items.Count, Is.EqualTo(10));
        });
    }

    [Test]
    public void E()
    {
        var entities = new ConcurrentDictionary<Int32, SurLaTable>();
        var creator = new CreationHandler<SurLaTable, Int32, Object>(entities, new CreationOptions<Int32>(() => entities.Count + 1));
        var searcher = new SearchHandler<SurLaTable, Int32, Object>(entities);
        var scope = new Object();
        for (var i = 0; i < 50; i++)
            creator.Create(new SurLaTable { Name = GetRandomName() }, scope);

        Assert.That(entities.Count, Is.EqualTo(50));

        var parameters = new[] { new FilterCriteria("Identifier", FilterOperation.LessThanOrEqual, 25) };
        var page = new Page(1, 50);
        var sort = new[] { new SortCriteria("Name", SortOrder.Descending) }.OrderBy(_ => 1);
        var found = searcher.Search(parameters, page, sort, scope);

        var items = found.Items.ToArray();
        for (var i = 1; i < items.Length; i++)
        {
            var current = items[i];
            var previous = items[i - 1];

            Assert.That(current.Name, Is.LessThanOrEqualTo(previous.Name));
        }
    }

    [Test]
    public void F()
    {
        var entities = new ConcurrentDictionary<Int32, SurLaTable>();
        var creator = new CreationHandler<SurLaTable, Int32, Object>(entities, new CreationOptions<Int32>(() => entities.Count + 1));
        var searcher = new SearchHandler<SurLaTable, Int32, Object>(entities);
        var scope = new Object();
        for (var i = 0; i < 50; i++)
            creator.Create(new SurLaTable { Name = GetRandomName() }, scope);

        Assert.That(entities.Count, Is.EqualTo(50));

        var parameters = new[] { new FilterCriteria("Identifier", FilterOperation.In, new[] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50 }) };
        var page = new Page(1, 50);
        var sort = new[] { new SortCriteria("Name", SortOrder.Descending) }.OrderBy(_ => 1);
        var found = searcher.Search(parameters, page, sort, scope);

        var items = found.Items.ToArray();
        for (var i = 1; i < items.Length; i++)
        {
            var current = items[i];
            var previous = items[i - 1];

            Assert.That(current.Name, Is.LessThanOrEqualTo(previous.Name));
        }
    }

    private static String GetRandomName(Int32 length = 5)
    {
        var name = new StringBuilder();
        for (var i = 0; i <= length; i++)
            name.Append((Char) new Random().Next('A', 'Z'));

        return name.ToString();
    }

    public class SurLaTable : EntityBase<Int32>, ICreated
    {
        public String Name { get; set; } = String.Empty;

        public DateTime CreationMomentUtc { get; set; }
    }
}
