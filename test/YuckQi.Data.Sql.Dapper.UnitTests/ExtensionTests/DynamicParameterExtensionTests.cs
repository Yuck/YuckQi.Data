using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using YuckQi.Data.Filtering;
using YuckQi.Data.Sql.Dapper.Extensions;

namespace YuckQi.Data.Sql.Dapper.UnitTests.ExtensionTests
{
    public class DynamicParameterExtensionTests
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void FilterCriteria_SingleValue_IsValid()
        {
            var criteria = new[] { new FilterCriteria("thing", "a test") };
            var parameters = criteria.ToDynamicParameters();

            Assert.AreEqual(1, parameters.ParameterNames.Count());
            Assert.AreEqual("thing", parameters.ParameterNames.First());
            Assert.AreEqual("a test", parameters.Get<Object>("thing"));
        }

        [Test]
        public void FilterCriteria_SingleNullValue_IsValid()
        {
            var criteria = new[] { new FilterCriteria("thing", null) };
            var parameters = criteria.ToDynamicParameters();

            Assert.AreEqual(1, parameters.ParameterNames.Count());
            Assert.AreEqual("thing", parameters.ParameterNames.First());
            Assert.IsNull(parameters.Get<Object>("thing"));
        }

        [Test]
        public void FilterCriteria_EmptyList_IsValid()
        {
            var parameters = new List<FilterCriteria>().ToDynamicParameters();

            Assert.AreEqual(0, parameters.ParameterNames.Count());
        }

        [Test]
        public void FilterCriteria_MultipleValues_IsValid()
        {
            var criteria = new[] { new FilterCriteria("thing", "a test"), new FilterCriteria("other", 1234.56M) };
            var parameters = criteria.ToDynamicParameters();

            Assert.AreEqual(2, parameters.ParameterNames.Count());
            Assert.AreEqual("thing", parameters.ParameterNames.First());
            Assert.AreEqual("a test", parameters.Get<Object>("thing"));
            Assert.AreEqual("other", parameters.ParameterNames.Last());
            Assert.AreEqual(1234.56M, parameters.Get<Object>("other"));
        }
    }
}
