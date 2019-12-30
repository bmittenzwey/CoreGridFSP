using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoreGridFSP.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.IO;
using Moq;
using Microsoft.AspNetCore.Http.Internal;
using System.Web;
using CoreGridFSP.Models;

namespace CoreGridFSP.Extensions.Tests
{
    [TestClass()]
    public class ExtractCoreGridOptionsExtensionsTests
    {
        [TestMethod()]
        public void EmptyQueryStringTest()
        {
            var request = CreateMockRequest("");
            var options = request.Object.ExtractCoreGridOptions();
            Assert.IsNull(options.SelectedSort);
            Assert.IsNull(options.SelectedSortName);
            Assert.AreEqual(SortableHeaderTagHelper.SortDirection.Asc, options.SelectedSortDirection);
            Assert.IsNotNull(options.PaginationOptions);
            Assert.IsNotNull(options.FilterList);
            Assert.AreEqual(0, options.FilterList.Count);
            Assert.AreEqual(10, options.PaginationOptions.PageSize);
            Assert.AreEqual(1, options.PaginationOptions.CurrentPage);
        }
        [TestMethod]
        public void SelectSortOnlyTest()
        {

            var request = CreateMockRequest("?selectedSort=TitleDesc");
            var options = request.Object.ExtractCoreGridOptions();
            Assert.AreEqual("TitleDesc", options.SelectedSort);
            Assert.AreEqual("Title", options.SelectedSortName);
            Assert.AreEqual(SortableHeaderTagHelper.SortDirection.Desc, options.SelectedSortDirection);
            Assert.IsNotNull(options.PaginationOptions);
            Assert.IsNotNull(options.FilterList);
            Assert.AreEqual(0, options.FilterList.Count);
        }
        [TestMethod]
        public void FilterOnlyTest()
        {

            var request = CreateMockRequest("?title=ghost");
            var options = request.Object.ExtractCoreGridOptions();
            Assert.IsNull(options.SelectedSort);
            Assert.IsNull(options.SelectedSortName);
            Assert.AreEqual(SortableHeaderTagHelper.SortDirection.Asc, options.SelectedSortDirection);
            Assert.IsNotNull(options.PaginationOptions);
            Assert.IsNotNull(options.FilterList);
            Assert.AreEqual(1, options.FilterList.Count);
            Assert.AreEqual("ghost", options.FilterList["Title"]);
        }
        [TestMethod]
        public void SortAndFilterTest()
        {

            var request = CreateMockRequest("?title=ghost&selectedSort=TitleDesc");
            var options = request.Object.ExtractCoreGridOptions();
            Assert.AreEqual("TitleDesc", options.SelectedSort);
            Assert.AreEqual("Title", options.SelectedSortName);
            Assert.AreEqual(SortableHeaderTagHelper.SortDirection.Desc, options.SelectedSortDirection);
            Assert.IsNotNull(options.PaginationOptions);
            Assert.IsNotNull(options.FilterList);
            Assert.AreEqual(1, options.FilterList.Count);
            Assert.AreEqual("ghost", options.FilterList["Title"]);
        }
        [TestMethod()]
        public void PaginationTest()
        {
            var request = CreateMockRequest("?currentPage=2&pagesize=33");
            var options = request.Object.ExtractCoreGridOptions();
            Assert.IsNull(options.SelectedSort);
            Assert.IsNull(options.SelectedSortName);
            Assert.AreEqual(SortableHeaderTagHelper.SortDirection.Asc, options.SelectedSortDirection);
            Assert.IsNotNull(options.PaginationOptions);
            Assert.IsNotNull(options.FilterList);
            Assert.AreEqual(0, options.FilterList.Count);
            Assert.AreEqual(33, options.PaginationOptions.PageSize);
            Assert.AreEqual(2, options.PaginationOptions.CurrentPage);
        }
        
        private static Mock<HttpRequest> CreateMockRequest(string queryString)
        {
            QueryString q = new QueryString(queryString);

            Dictionary<string, Microsoft.Extensions.Primitives.StringValues> qDict = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>();
            var qList = HttpUtility.ParseQueryString(queryString);
            
            foreach (var k in qList.AllKeys)
            {
                if(!qDict.ContainsKey(k))
                    qDict.Add(k, qList[k]);
            }

            QueryCollection qc = new QueryCollection(qDict);

            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(x => x.QueryString).Returns(q);
            mockRequest.Setup(x => x.Query).Returns(qc);
            return mockRequest;
        }
    }
}