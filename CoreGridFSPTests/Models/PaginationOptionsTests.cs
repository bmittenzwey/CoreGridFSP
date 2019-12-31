using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoreGridFSP.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreGridFSP.Extensions.Tests
{
    [TestClass()]
    public class PaginationOptionsTests
    {
        [TestMethod()]
        public void TotalPagesTest()
        {
            var p = new PaginationOptions();
            Assert.AreEqual(1, p.TotalPages);
            p.Count = 15;
            Assert.AreEqual(2, p.TotalPages);

        }
        [TestMethod()]
        public void TotalPagesZeroPageSizeTest()
        {
            var p = new PaginationOptions();
            p.PageSize = 0;
            p.Count = 1500;
            Assert.AreEqual(1, p.TotalPages);

        }
        [TestMethod()]
        public void EnablePreviousTest()
        {
            var p = new PaginationOptions();
            p.PageSize = 0;
            p.CurrentPage = 1;
            p.Count = 1500;
            Assert.IsFalse(p.EnablePrevious);

            p.CurrentPage = 2;
            Assert.IsTrue(p.EnablePrevious);

        }
        [TestMethod()]
        public void EnableNextTest()
        {
            var p = new PaginationOptions();
            p.PageSize = 0;
            p.CurrentPage = 1;
            p.Count = 1500;
            Assert.IsFalse(p.EnableNext);

            p.CurrentPage = 2;
            Assert.IsFalse(p.EnableNext);

            p.PageSize = 5;
            p.Count = 20;
            Assert.IsTrue(p.EnableNext);

            p.CurrentPage = 1;
            Assert.IsTrue(p.EnableNext);

            p.CurrentPage = 4;
            Assert.IsFalse(p.EnableNext);
        }
    }


}