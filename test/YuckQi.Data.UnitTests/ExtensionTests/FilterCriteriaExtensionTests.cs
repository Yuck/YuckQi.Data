using NUnit.Framework;
using YuckQi.Data.Extensions;
using YuckQi.Data.Filtering;

namespace YuckQi.Data.UnitTests.ExtensionTests;

public class FilterCriteriaExtensionTests
{
    private static readonly Object? NullObject = null;

    [SetUp]
    public void Setup() { }

    [Test]
    public void Object_WithSingleProperty_IsValid()
    {
        var parameters = new { thing = "a test" };
        var filters = parameters.ToFilterCollection();

        Assert.Multiple(() =>
        {
            Assert.That(filters.Count, Is.EqualTo(1));
            Assert.That(filters.First().Value, Is.EqualTo("a test"));
        });
    }

    [Test]
    public void Object_WithSingleNullProperty_IsValid()
    {
        var parameters = new { thing = (Int32?) null };
        var filters = parameters.ToFilterCollection();

        Assert.Multiple(() =>
        {
            Assert.That(filters.Count, Is.EqualTo(1));
            Assert.That(filters.First().Value, Is.Null);
        });
    }

    [Test]
    public void Object_WithNoProperties_IsValid()
    {
        var parameters = new { };
        var filters = parameters.ToFilterCollection();

        Assert.That(filters.Count, Is.EqualTo(0));
    }

    [Test]
    public void Object_WithMultipleProperties_IsValid()
    {
        var parameters = new { thing = "a test", other = 1234.56M };
        var filters = parameters.ToFilterCollection();

        Assert.Multiple(() =>
        {
            Assert.That(filters.Count, Is.EqualTo(2));
            Assert.That(filters.First().Value, Is.EqualTo("a test"));
            Assert.That(filters.Last().Value, Is.EqualTo(1234.56M));
        });
    }

    [Test]
    public void Null_IsValid()
    {
        var parameters = NullObject;
        var filters = parameters.ToFilterCollection();

        Assert.That(filters.Count, Is.Zero);
    }

    [Test]
    public void FilterCriteria_Single_IsValid()
    {
        var parameters = new FilterCriteria("SomeField", 1234);
        var filters = parameters.ToFilterCollection();

        Assert.Multiple(() =>
        {
            Assert.That(filters.Count, Is.EqualTo(1));
            Assert.That(filters.First().Value, Is.EqualTo(1234));
        });
    }

    [Test]
    public void FilterCriteria_Multiple_IsValid()
    {
        var parameters = new List<FilterCriteria> { new("SomeField", 1234), new("AnotherOne", "hi") };
        var filters = parameters.ToFilterCollection();

        Assert.Multiple(() =>
        {
            Assert.That(filters.Count, Is.EqualTo(2));
            Assert.That(filters.First().Value, Is.EqualTo(1234));
            Assert.That(filters.Last().Value, Is.EqualTo("hi"));
        });
    }
}
