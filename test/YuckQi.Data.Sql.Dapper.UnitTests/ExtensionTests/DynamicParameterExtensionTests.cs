using NUnit.Framework;
using YuckQi.Data.Filtering;
using YuckQi.Data.Sql.Dapper.Extensions;

namespace YuckQi.Data.Sql.Dapper.UnitTests.ExtensionTests;

public class DynamicParameterExtensionTests
{
    [SetUp]
    public void Setup() { }

    [Test]
    public void FilterCriteria_SingleValue_IsValid()
    {
        var criteria = new[] { new FilterCriteria("thing", "a test") };
        var parameters = criteria.ToDynamicParameters();

        Assert.Multiple(() =>
        {
            Assert.That(parameters.ParameterNames.Count(), Is.EqualTo(1));
            Assert.That(parameters.ParameterNames.First(), Is.EqualTo("thing"));
            Assert.That(parameters.Get<Object>("thing"), Is.EqualTo("a test"));
        });
    }

    [Test]
    public void FilterCriteria_SingleNullValue_IsValid()
    {
        var criteria = new[] { new FilterCriteria("thing", null) };
        var parameters = criteria.ToDynamicParameters();

        Assert.Multiple(() =>
        {
            Assert.That(parameters.ParameterNames.Count(), Is.EqualTo(1));
            Assert.That(parameters.ParameterNames.First(), Is.EqualTo("thing"));
            Assert.That(parameters.Get<Object>("thing"), Is.Null);
        });
    }

    [Test]
    public void FilterCriteria_EmptyList_IsValid()
    {
        var parameters = new List<FilterCriteria>().ToDynamicParameters();

        Assert.That(parameters.ParameterNames.Count(), Is.Zero);
    }

    [Test]
    public void FilterCriteria_MultipleValues_IsValid()
    {
        var criteria = new[] { new FilterCriteria("thing", "a test"), new FilterCriteria("other", 1234.56M) };
        var parameters = criteria.ToDynamicParameters();

        Assert.Multiple(() =>
        {
            Assert.That(parameters.ParameterNames.Count(), Is.EqualTo(2));
            Assert.That(parameters.ParameterNames.First(), Is.EqualTo("thing"));
            Assert.That(parameters.Get<Object>("thing"), Is.EqualTo("a test"));
            Assert.That(parameters.ParameterNames.Last(), Is.EqualTo("other"));
            Assert.That(parameters.Get<Object>("other"), Is.EqualTo(1234.56M));
        });
    }
}
