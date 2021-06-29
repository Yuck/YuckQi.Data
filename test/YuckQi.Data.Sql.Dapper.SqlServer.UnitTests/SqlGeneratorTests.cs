using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using NUnit.Framework;
using YuckQi.Data.Sorting;
using YuckQi.Domain.ValueObjects;
using SortOrder = YuckQi.Data.Sorting.SortOrder;

namespace YuckQi.Data.Sql.Dapper.SqlServer.UnitTests
{
    public class SqlGeneratorTests
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void GenerateCountQuery_WithSingleParameter_IsValid()
        {
            var generator = new SqlGenerator<SurLaTableRecord>();
            var parameters = new[] { new SqlParameter("Id", SqlDbType.BigInt) { Value = 1 } };
            var sql = generator.GenerateCountQuery(parameters).Replace(Environment.NewLine, " ");

            Assert.AreEqual(sql, "select count(*) from [dbo].[SurLaTableRecord] where ([Id] = @Id);");
        }

        [Test]
        public void GenerateGetQuery_WithSingleParameter_IsValid()
        {
            var generator = new SqlGenerator<SurLaTableRecord>();
            var parameters = new[] { new SqlParameter("Id", SqlDbType.BigInt) { Value = 1 } };
            var sql = generator.GenerateGetQuery(parameters).Replace(Environment.NewLine, " ");

            Assert.AreEqual(sql, "select [Id], [Name] from [dbo].[SurLaTableRecord] where ([Id] = @Id);");
        }

        [Test]
        public void GenerateSearchQuery_WithSingleParameter_IsValid()
        {
            var generator = new SqlGenerator<SurLaTableRecord>();
            var parameters = new[] { new SqlParameter("Name", SqlDbType.BigInt) { Value = "" } };
            var page = new Page(2, 50);
            var sort = new List<SortCriteria> { new("Name", SortOrder.Descending) }.OrderBy(t => t);
            var sql = generator.GenerateSearchQuery(parameters, page, sort).Replace(Environment.NewLine, " ");

            Assert.AreEqual(sql, "select [Id], [Name] from [dbo].[SurLaTableRecord] where ([Name] = @Name) order by [Name] desc offset 50 rows fetch first 50 rows only;");
        }
    }

    public class SurLaTableRecord
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
    }
}