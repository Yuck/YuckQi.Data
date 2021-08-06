using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using YuckQi.Data.Extensions;
using YuckQi.Data.Filtering;

namespace YuckQi.Data.UnitTests.ExtensionTests
{
    public class FilterCriteriaExtensionTests
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void Object_WithSingleProperty_IsValid()
        {
            var parameters = new { thing = "a test" };
            var filters = parameters.ToFilterCollection();

            Assert.AreEqual(1, filters.Count);
            Assert.AreEqual("a test", filters.First().Value);
        }

        [Test]
        public void Object_WithSingleNullProperty_IsValid()
        {
            var parameters = new { thing = (Int32?) null };
            var filters = parameters.ToFilterCollection();

            Assert.AreEqual(1, filters.Count);
            Assert.IsNull(filters.First().Value);
        }

        [Test]
        public void Object_WithNoProperties_IsValid()
        {
            var parameters = new { };
            var filters = parameters.ToFilterCollection();

            Assert.AreEqual(0, filters.Count);
        }

        [Test]
        public void Object_WithMultipleProperties_IsValid()
        {
            var parameters = new { thing = "a test", other = 1234.56M };
            var filters = parameters.ToFilterCollection();

            Assert.AreEqual(2, filters.Count);
            Assert.AreEqual("a test", filters.First().Value);
            Assert.AreEqual(1234.56M, filters.Last().Value);
        }

        [Test]
        public void Null_IsValid()
        {
            var parameters = (Object) null;
            var filters = parameters.ToFilterCollection();

            Assert.AreEqual(0, filters.Count);
        }

        [Test]
        public void FilterCriteria_Single_IsValid()
        {
            var parameters = new FilterCriteria("SomeField", 1234);
            var filters = parameters.ToFilterCollection();

            Assert.AreEqual(1, filters.Count);
            Assert.AreEqual(1234, filters.First().Value);
        }

        [Test]
        public void FilterCriteria_Multiple_IsValid()
        {
            var parameters = new List<FilterCriteria> { new("SomeField", 1234), new("AnotherOne", "hi") };
            var filters = parameters.ToFilterCollection();

            Assert.AreEqual(2, filters.Count);
            Assert.AreEqual(1234, filters.First().Value);
            Assert.AreEqual("hi", filters.Last().Value);
        }
    }
}
