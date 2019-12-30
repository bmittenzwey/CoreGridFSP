using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoreGridFSP.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CoreGridFSP.Extensions.Tests
{
    [TestClass()]
    public class QueryExtensionsTests
    {
        protected List<DataModel> list = new List<DataModel>()
            {
                new DataModel{Id=2, Name="Allen"},
                new DataModel{Id=1, Name = "Zach"},
                new DataModel{Id=3, Name = "Tim"},
                new DataModel {Id=4, Name = "Joe"}
            };

        [TestMethod()]
        public void OrderByIntAscTest()
        {
           
            //Asc by Id
            StringBuilder sb = new StringBuilder();
            list.AsQueryable().OrderBy("Id", false).Select(x => x.Id).ToList().ForEach(x => sb.Append(x));
            Assert.AreEqual("1234", sb.ToString());

            
        }
        [TestMethod()]
        public void OrderByStringDescTest()
        {
            //Desc by name
            StringBuilder sb = new StringBuilder();
            list.AsQueryable().OrderBy("Name", true).Select(x => x.Id).ToList().ForEach(x => sb.Append(x));
            Assert.AreEqual("1342", sb.ToString());

            
        }
        [TestMethod()]
        public void CaseInsensitiveOrderByStringAscTest()
        {

            //Case insensitive by name
            StringBuilder sb = new StringBuilder();
            list.AsQueryable().OrderBy("name", false).Select(x => x.Id).ToList().ForEach(x => sb.Append(x));
            Assert.AreEqual("2431", sb.ToString());
        }
    }
    public class DataModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}