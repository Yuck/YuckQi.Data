using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using NUnit.Framework;
using YuckQi.Data.Filtering;
using YuckQi.Data.Sorting;
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
    }

    [Table("SurLaTable")]
    public class SurLaTableRecord
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
    }
}
