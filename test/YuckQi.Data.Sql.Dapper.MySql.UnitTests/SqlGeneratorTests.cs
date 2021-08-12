using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using NUnit.Framework;
using YuckQi.Data.Filtering;
using YuckQi.Data.Sorting;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Domain.ValueObjects;

namespace YuckQi.Data.Sql.Dapper.MySql.UnitTests
{
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

            Assert.AreEqual("select count(*) from `SurLaTable` where (`Id` = @Id);", sql);
        }

        [Test]
        public void GenerateGetQuery_WithSingleParameter_IsValid()
        {
            var generator = new SqlGenerator<SurLaTableRecord>();
            var parameters = new[] { new FilterCriteria("Id", 1) };
            var sql = generator.GenerateGetQuery(parameters).Replace(Environment.NewLine, " ");

            Assert.AreEqual("select `Id`, `Name` from `SurLaTable` where (`Id` = @Id);", sql);
        }

        [Test]
        public void GenerateSearchQuery_WithSingleParameter_IsValid()
        {
            var generator = new SqlGenerator<SurLaTableRecord>();
            var parameters = new[] { new FilterCriteria("Name", "Some Guy") };
            var page = new Page(2, 50);
            var sort = new List<SortCriteria> { new("Name", SortOrder.Descending) }.OrderBy(t => t);
            var sql = generator.GenerateSearchQuery(parameters, page, sort).Replace(Environment.NewLine, " ");

            Assert.AreEqual("select `Id`, `Name` from `SurLaTable` where (`Name` = @Name) order by `Name` desc limit 50 offset 50;", sql);
        }

        [Test]
        public void GenerateGetQuery_WithEqualOperation_IsValid()
        {
            var generator = new SqlGenerator<SurLaTableRecord>();
            var parameters = new[] { new FilterCriteria("Name", "Some Guy") };
            var sql = generator.GenerateGetQuery(parameters).Replace(Environment.NewLine, " ");

            Assert.AreEqual("select `Id`, `Name` from `SurLaTable` where (`Name` = @Name);", sql);
        }

        [Test]
        public void GenerateGetQuery_WithEqualOperation_AndNullValue_IsValid()
        {
            var generator = new SqlGenerator<SurLaTableRecord>();
            var parameters = new[] { new FilterCriteria("Name", null) };
            var sql = generator.GenerateGetQuery(parameters).Replace(Environment.NewLine, " ");

            Assert.AreEqual("select `Id`, `Name` from `SurLaTable` where (`Name` is null);", sql);
        }

        [Test]
        public void GenerateGetQuery_WithNotEqualOperation_IsValid()
        {
            var generator = new SqlGenerator<SurLaTableRecord>();
            var parameters = new[] { new FilterCriteria("Name", FilterOperation.NotEqual, "Some Guy") };
            var sql = generator.GenerateGetQuery(parameters).Replace(Environment.NewLine, " ");

            Assert.AreEqual("select `Id`, `Name` from `SurLaTable` where (`Name` != @Name);", sql);
        }

        [Test]
        public void GenerateGetQuery_WithNotEqualOperation_AndNullValue_IsValid()
        {
            var generator = new SqlGenerator<SurLaTableRecord>();
            var parameters = new[] { new FilterCriteria("Name", FilterOperation.NotEqual, null) };
            var sql = generator.GenerateGetQuery(parameters).Replace(Environment.NewLine, " ");

            Assert.AreEqual("select `Id`, `Name` from `SurLaTable` where (`Name` is not null);", sql);
        }

        [Test]
        public void GenerateGetQuery_WithGreaterThanOperation_IsValid()
        {
            var generator = new SqlGenerator<SurLaTableRecord>();
            var parameters = new[] { new FilterCriteria("Name", FilterOperation.GreaterThan, 1234) };
            var sql = generator.GenerateGetQuery(parameters).Replace(Environment.NewLine, " ");

            Assert.AreEqual("select `Id`, `Name` from `SurLaTable` where (`Name` > @Name);", sql);
        }

        [Test]
        public void GenerateGetQuery_WithGreaterThanOrEqualOperation_IsValid()
        {
            var generator = new SqlGenerator<SurLaTableRecord>();
            var parameters = new[] { new FilterCriteria("Name", FilterOperation.GreaterThanOrEqual, 1234) };
            var sql = generator.GenerateGetQuery(parameters).Replace(Environment.NewLine, " ");

            Assert.AreEqual("select `Id`, `Name` from `SurLaTable` where (`Name` >= @Name);", sql);
        }

        [Test]
        public void GenerateGetQuery_WithLessThanOperation_IsValid()
        {
            var generator = new SqlGenerator<SurLaTableRecord>();
            var parameters = new[] { new FilterCriteria("Name", FilterOperation.LessThan, 1234) };
            var sql = generator.GenerateGetQuery(parameters).Replace(Environment.NewLine, " ");

            Assert.AreEqual("select `Id`, `Name` from `SurLaTable` where (`Name` < @Name);", sql);
        }

        [Test]
        public void GenerateGetQuery_WithLessThanOrEqualOperation_IsValid()
        {
            var generator = new SqlGenerator<SurLaTableRecord>();
            var parameters = new[] { new FilterCriteria("Name", FilterOperation.LessThanOrEqual, 1234) };
            var sql = generator.GenerateGetQuery(parameters).Replace(Environment.NewLine, " ");

            Assert.AreEqual("select `Id`, `Name` from `SurLaTable` where (`Name` <= @Name);", sql);
        }
    }

    public class SurLaTable : EntityBase<Int32>, ICreated, IRevised
    {
        public String Name { get; set; }

        public DateTime CreationMomentUtc { get; set; }
        public DateTime RevisionMomentUtc { get; set; }
    }

    [Table("SurLaTable")]
    public class SurLaTableRecord
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
    }
}
