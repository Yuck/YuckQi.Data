using Dapper;
using NUnit.Framework;
using YuckQi.Data.Filtering;
using YuckQi.Data.Sorting;
using YuckQi.Domain.ValueObjects;

namespace YuckQi.Data.Sql.Dapper.SqlServer.UnitTests;

public class SqlGeneratorTests
{
    [SetUp]
    public void Setup() { }

    [Test]
    public void GenerateCountQuery_WithSingleParameter_IsValid()
    {
        var generator = new SqlGenerator<SurLaTableRecord>();
        var parameters = new[] { new FilterCriteria("Id", 1) };
        var sql = generator.GenerateCountQuery(parameters).Replace(Environment.NewLine, " ");

        Assert.That(sql, Is.EqualTo("select count(*) from [dbo].[SurLaTable] where ([Id] = @Id);"));
    }

    [Test]
    public void GenerateGetQuery_WithSingleParameter_IsValid()
    {
        var generator = new SqlGenerator<SurLaTableRecord>();
        var parameters = new[] { new FilterCriteria("Id", 1) };
        var sql = generator.GenerateGetQuery(parameters).Replace(Environment.NewLine, " ");

        Assert.That(sql, Is.EqualTo("select [Id], [Name] from [dbo].[SurLaTable] where ([Id] = @Id);"));
    }

    [Test]
    public void GenerateSearchQuery_WithSingleParameter_IsValid()
    {
        var generator = new SqlGenerator<SurLaTableRecord>();
        var parameters = new[] { new FilterCriteria("Name", "Some Guy") };
        var page = new Page(2, 50);
        var sort = new List<SortCriteria> { new("Name", SortOrder.Descending) }.OrderBy(t => t);
        var sql = generator.GenerateSearchQuery(parameters, page, sort).Replace(Environment.NewLine, " ");

        Assert.That(sql, Is.EqualTo("select [Id], [Name] from [dbo].[SurLaTable] where ([Name] = @Name) order by [Name] desc offset 50 rows fetch first 50 rows only;"));
    }

    [Test]
    public void GenerateGetQuery_WithEqualOperation_IsValid()
    {
        var generator = new SqlGenerator<SurLaTableRecord>();
        var parameters = new[] { new FilterCriteria("Name", "Some Guy") };
        var sql = generator.GenerateGetQuery(parameters).Replace(Environment.NewLine, " ");

        Assert.That(sql, Is.EqualTo("select [Id], [Name] from [dbo].[SurLaTable] where ([Name] = @Name);"));
    }

    [Test]
    public void GenerateGetQuery_WithEqualOperation_AndNullValue_IsValid()
    {
        var generator = new SqlGenerator<SurLaTableRecord>();
        var parameters = new[] { new FilterCriteria("Name", null) };
        var sql = generator.GenerateGetQuery(parameters).Replace(Environment.NewLine, " ");

        Assert.That(sql, Is.EqualTo("select [Id], [Name] from [dbo].[SurLaTable] where ([Name] is null);"));
    }

    [Test]
    public void GenerateGetQuery_WithNotEqualOperation_IsValid()
    {
        var generator = new SqlGenerator<SurLaTableRecord>();
        var parameters = new[] { new FilterCriteria("Name", FilterOperation.NotEqual, "Some Guy") };
        var sql = generator.GenerateGetQuery(parameters).Replace(Environment.NewLine, " ");

        Assert.That(sql, Is.EqualTo("select [Id], [Name] from [dbo].[SurLaTable] where ([Name] != @Name);"));
    }

    [Test]
    public void GenerateGetQuery_WithNotEqualOperation_AndNullValue_IsValid()
    {
        var generator = new SqlGenerator<SurLaTableRecord>();
        var parameters = new[] { new FilterCriteria("Name", FilterOperation.NotEqual, null) };
        var sql = generator.GenerateGetQuery(parameters).Replace(Environment.NewLine, " ");

        Assert.That(sql, Is.EqualTo("select [Id], [Name] from [dbo].[SurLaTable] where ([Name] is not null);"));
    }

    [Test]
    public void GenerateGetQuery_WithGreaterThanOperation_IsValid()
    {
        var generator = new SqlGenerator<SurLaTableRecord>();
        var parameters = new[] { new FilterCriteria("Name", FilterOperation.GreaterThan, 1234) };
        var sql = generator.GenerateGetQuery(parameters).Replace(Environment.NewLine, " ");

        Assert.That(sql, Is.EqualTo("select [Id], [Name] from [dbo].[SurLaTable] where ([Name] > @Name);"));
    }

    [Test]
    public void GenerateGetQuery_WithGreaterThanOrEqualOperation_IsValid()
    {
        var generator = new SqlGenerator<SurLaTableRecord>();
        var parameters = new[] { new FilterCriteria("Name", FilterOperation.GreaterThanOrEqual, 1234) };
        var sql = generator.GenerateGetQuery(parameters).Replace(Environment.NewLine, " ");

        Assert.That(sql, Is.EqualTo("select [Id], [Name] from [dbo].[SurLaTable] where ([Name] >= @Name);"));
    }

    [Test]
    public void GenerateGetQuery_WithLessThanOperation_IsValid()
    {
        var generator = new SqlGenerator<SurLaTableRecord>();
        var parameters = new[] { new FilterCriteria("Name", FilterOperation.LessThan, 1234) };
        var sql = generator.GenerateGetQuery(parameters).Replace(Environment.NewLine, " ");

        Assert.That(sql, Is.EqualTo("select [Id], [Name] from [dbo].[SurLaTable] where ([Name] < @Name);"));
    }

    [Test]
    public void GenerateGetQuery_WithLessThanOrEqualOperation_IsValid()
    {
        var generator = new SqlGenerator<SurLaTableRecord>();
        var parameters = new[] { new FilterCriteria("Name", FilterOperation.LessThanOrEqual, 1234) };
        var sql = generator.GenerateGetQuery(parameters).Replace(Environment.NewLine, " ");

        Assert.That(sql, Is.EqualTo("select [Id], [Name] from [dbo].[SurLaTable] where ([Name] <= @Name);"));
    }

    [Table("SurLaTable")]
    public class SurLaTableRecord
    {
        public Int32 Id { get; set; }
        public String Name { get; set; } = String.Empty;
    }
}
