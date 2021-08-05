using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using NUnit.Framework;
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
            var generator = new SqlGenerator<ThatThingRecord>();
            var parameters = new[] { new MySqlParameter("Id", MySqlDbType.Int64) { Value = 1 } };
            var sql = generator.GenerateCountQuery(parameters).Replace(Environment.NewLine, " ");

            Assert.AreEqual(sql, "select count(*) from `ThatThingRecord` where (`Id` = @Id);");
        }

        [Test]
        public void GenerateGetQuery_WithSingleParameter_IsValid()
        {
            var generator = new SqlGenerator<ThatThingRecord>();
            var parameters = new[] { new MySqlParameter("Id", MySqlDbType.Int64) { Value = 1 } };
            var sql = generator.GenerateGetQuery(parameters).Replace(Environment.NewLine, " ");

            Assert.AreEqual(sql, "select `Id`, `Name` from `ThatThingRecord` where (`Id` = @Id);");
        }

        [Test]
        public void GenerateSearchQuery_WithSingleParameter_IsValid()
        {
            var generator = new SqlGenerator<ThatThingRecord>();
            var parameters = new[] { new MySqlParameter("Name", MySqlDbType.Int64) { Value = "" } };
            var page = new Page(2, 50);
            var sort = new List<SortCriteria> { new("Name", SortOrder.Descending) }.OrderBy(t => t);
            var sql = generator.GenerateSearchQuery(parameters, page, sort).Replace(Environment.NewLine, " ");

            Assert.AreEqual(sql, "select `Id`, `Name` from `ThatThingRecord` where (`Name` = @Name) order by `Name` desc limit 50 offset 50;");
        }
    }

    public class ThatThingRecord
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
    }
}
