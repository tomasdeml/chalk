// Source: http://sysmagazine.com/posts/156325/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

// ReSharper disable once CheckNamespace
namespace NUnit.Specification
{
    public abstract class SpecificationBase
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            Setup();
            Given();
            When();
        }

        public virtual void Setup() { }

        private void Given()
        {
            IEnumerable<MethodInfo> publicMethods = GetType().GetMethods();

            publicMethods.Where(m => m.GetCustomAttributes(typeof(GivenAttribute), false).Count() != 0)
                .ToList()
                .ForEach(m => m.Invoke(this, new object[0]));

            publicMethods.Where(m => m.GetCustomAttributes(typeof(AndAttribute), false).Count() != 0)
                .ToList()
                .ForEach(m => m.Invoke(this, new object[0]));
        }

        private void When()
        {
            GetType()
                .GetMethods()
                .Where(m => m.GetCustomAttributes(typeof (WhenAttribute), false).Count() != 0)
                .ToList()
                .ForEach(m => m.Invoke(this, new object[0]));
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            Teardown();
        }

        public virtual void Teardown() { }
    }

    public class SpecificationAttribute : TestFixtureAttribute { }
    public class GivenAttribute : Attribute { }
    public class AndAttribute : Attribute { }
    public class WhenAttribute : Attribute { }
    public class ThenAttribute : TestAttribute { }
}