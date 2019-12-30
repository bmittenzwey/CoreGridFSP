using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoreGridFSP.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreGridFSP.Extensions.Tests
{
    [TestClass()]
    public class InheritedModelExtensionsTests
    {
        [TestMethod()]
        public void ShallowCopyTest()
        {
            var p = new ParentModel
            {
                Id = 1,
                Name = "Test"
            };

            var c = new ChildModel();

            c.ShallowCopy(p);

            Assert.IsNotNull(c);
            Assert.AreEqual(p.Id, c.Id);
            Assert.AreEqual(p.Name, c.Name);
            Assert.IsNull(c.Description);
        }
        [TestMethod()]
        public void ShallowCopyMismatchedTest()
        {
            var s = new Sibling
            {
                Id = 1,
                Name = "Test",
                Age=3

            };

            var c = new ChildModel();

            c.ShallowCopy(s);

            Assert.IsNotNull(c);
            Assert.AreEqual(s.Id, c.Id);
            Assert.AreEqual(s.Name, c.Name);
            Assert.IsNull(c.Description);
        }
    }

    public class ParentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class ChildModel : ParentModel
    {
        public string Description { get; set; }
    }
    public class Sibling : ParentModel
    {
        public int Age { get; set; }
    }
}