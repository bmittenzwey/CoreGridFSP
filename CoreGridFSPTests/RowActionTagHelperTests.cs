using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoreGridFSP;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreGridFSP.Models;

namespace CoreGridFSP.Tests
{
    [TestClass()]
    public class RowActionTagHelperTests
    {
        [TestMethod()]
        public void RowActionTagHelperTest()
        {
            var mg = MockHtmlGenerator();
            var r = new RowActionTagHelper(mg.Object);

            Assert.IsTrue(r.CanDelete);
            Assert.IsTrue(r.CanEdit);
            Assert.IsTrue(r.CanView);
            Assert.AreEqual("btn-outline-secondary btn-sm", r.ButtonClass);
            Assert.AreEqual("Action", r.LableName);
            Assert.IsNull(r.Options);
            Assert.IsNotNull(r.RouteValues);
            Assert.AreEqual(0, r.RouteValues.Count);
        }

        [TestMethod()]
        public void ProcessDefaultTest()
        {
            var mg = MockHtmlGenerator();
            var viewContext = GetViewContext();
            var Routes = new Dictionary<string, string>();
            Routes.Add("pageSize", "10");
            Routes.Add("id", "1");


            var editTag = new TagBuilder("div");
            editTag.InnerHtml.Append("Edit");
            mg.Setup(x => x.GenerateActionLink(viewContext, "Edit", "Edit", null, null, null, null, Routes,  It.IsAny<Object>())).Returns(editTag);
            var deleteTag = new TagBuilder("div");
            deleteTag.InnerHtml.Append("Delete");
            mg.Setup(x => x.GenerateActionLink(viewContext, "Delete", "Delete", null, null, null, null, Routes, It.IsAny<Object>())).Returns(deleteTag);
            var detailsTag = new TagBuilder("div");
            detailsTag.InnerHtml.Append("Details");
            mg.Setup(x => x.GenerateActionLink(viewContext, "Details", "Details", null, null, null, null, Routes, It.IsAny<Object>())).Returns(detailsTag);

            var options = new CoreGridFSPOptions();


            var r = new RowActionTagHelper(mg.Object);
           
            r.RouteValues = Routes;
            r.Options = options;
            r.ViewContext = viewContext;

            var output = GetTagHelperOutput();

            r.Process(GetTagHelperContext(), output);

            Assert.IsNotNull(output);
            var expected = "<button class=\"btn-sm btn-outline-secondary btn\">Action</button><button aria-expanded=\"false\" aria-haspopup=\"true\" class=\"dropdown-toggle-split dropdown-toggle btn-outline-secondary btn\" data-toggle=\"dropdown\" type=\"button\"><span class=\"sr-only\">Toggle Dropdown</span></button><div class=\"dropdown-menu\"><div>Edit</div><div>Delete</div><div>Details</div></div>";
            Assert.AreEqual(expected, output.Content.GetContent());
        }
        [TestMethod()]
        public void ProcessNoEditTest()
        {
            var mg = MockHtmlGenerator();
            var viewContext = GetViewContext();
            var Routes = new Dictionary<string, string>();
            Routes.Add("pageSize", "10");
            Routes.Add("id", "1");


            var editTag = new TagBuilder("div");
            editTag.InnerHtml.Append("Edit");
            mg.Setup(x => x.GenerateActionLink(viewContext, "Edit", "Edit", null, null, null, null, Routes, It.IsAny<Object>())).Returns(editTag);
            var deleteTag = new TagBuilder("div");
            deleteTag.InnerHtml.Append("Delete");
            mg.Setup(x => x.GenerateActionLink(viewContext, "Delete", "Delete", null, null, null, null, Routes, It.IsAny<Object>())).Returns(deleteTag);
            var detailsTag = new TagBuilder("div");
            detailsTag.InnerHtml.Append("Details");
            mg.Setup(x => x.GenerateActionLink(viewContext, "Details", "Details", null, null, null, null, Routes, It.IsAny<Object>())).Returns(detailsTag);

            var options = new CoreGridFSPOptions();


            var r = new RowActionTagHelper(mg.Object);
            r.CanEdit = false;
            r.RouteValues = Routes;
            r.Options = options;
            r.ViewContext = viewContext;

            var output = GetTagHelperOutput();

            r.Process(GetTagHelperContext(), output);

            Assert.IsNotNull(output);
            var expected = "<button class=\"btn-sm btn-outline-secondary btn\">Action</button><button aria-expanded=\"false\" aria-haspopup=\"true\" class=\"dropdown-toggle-split dropdown-toggle btn-outline-secondary btn\" data-toggle=\"dropdown\" type=\"button\"><span class=\"sr-only\">Toggle Dropdown</span></button><div class=\"dropdown-menu\"><div>Delete</div><div>Details</div></div>";
            Assert.AreEqual(expected, output.Content.GetContent());
        }
        [TestMethod()]
        public void ProcessNoDeleteTest()
        {
            var mg = MockHtmlGenerator();
            var viewContext = GetViewContext();
            var Routes = new Dictionary<string, string>();
            Routes.Add("pageSize", "10");
            Routes.Add("id", "1");


            var editTag = new TagBuilder("div");
            editTag.InnerHtml.Append("Edit");
            mg.Setup(x => x.GenerateActionLink(viewContext, "Edit", "Edit", null, null, null, null, Routes, It.IsAny<Object>())).Returns(editTag);
            var deleteTag = new TagBuilder("div");
            deleteTag.InnerHtml.Append("Delete");
            mg.Setup(x => x.GenerateActionLink(viewContext, "Delete", "Delete", null, null, null, null, Routes, It.IsAny<Object>())).Returns(deleteTag);
            var detailsTag = new TagBuilder("div");
            detailsTag.InnerHtml.Append("Details");
            mg.Setup(x => x.GenerateActionLink(viewContext, "Details", "Details", null, null, null, null, Routes, It.IsAny<Object>())).Returns(detailsTag);

            var options = new CoreGridFSPOptions();


            var r = new RowActionTagHelper(mg.Object);
            r.CanDelete = false;
            r.RouteValues = Routes;
            r.Options = options;
            r.ViewContext = viewContext;

            var output = GetTagHelperOutput();

            r.Process(GetTagHelperContext(), output);

            Assert.IsNotNull(output);
            var expected = "<button class=\"btn-sm btn-outline-secondary btn\">Action</button><button aria-expanded=\"false\" aria-haspopup=\"true\" class=\"dropdown-toggle-split dropdown-toggle btn-outline-secondary btn\" data-toggle=\"dropdown\" type=\"button\"><span class=\"sr-only\">Toggle Dropdown</span></button><div class=\"dropdown-menu\"><div>Edit</div><div>Details</div></div>";
            Assert.AreEqual(expected, output.Content.GetContent());
        }
        [TestMethod()]
        public void ProcessNoViewTest()
        {
            var mg = MockHtmlGenerator();
            var viewContext = GetViewContext();
            var Routes = new Dictionary<string, string>();
            Routes.Add("pageSize", "10");
            Routes.Add("id", "1");


            var editTag = new TagBuilder("div");
            editTag.InnerHtml.Append("Edit");
            mg.Setup(x => x.GenerateActionLink(viewContext, "Edit", "Edit", null, null, null, null, Routes, It.IsAny<Object>())).Returns(editTag);
            var deleteTag = new TagBuilder("div");
            deleteTag.InnerHtml.Append("Delete");
            mg.Setup(x => x.GenerateActionLink(viewContext, "Delete", "Delete", null, null, null, null, Routes, It.IsAny<Object>())).Returns(deleteTag);
            var detailsTag = new TagBuilder("div");
            detailsTag.InnerHtml.Append("Details");
            mg.Setup(x => x.GenerateActionLink(viewContext, "Details", "Details", null, null, null, null, Routes, It.IsAny<Object>())).Returns(detailsTag);

            var options = new CoreGridFSPOptions();


            var r = new RowActionTagHelper(mg.Object);
            r.CanView = false;
            r.RouteValues = Routes;
            r.Options = options;
            r.ViewContext = viewContext;

            var output = GetTagHelperOutput();

            r.Process(GetTagHelperContext(), output);

            Assert.IsNotNull(output);
            var expected = "<button class=\"btn-sm btn-outline-secondary btn\">Action</button><button aria-expanded=\"false\" aria-haspopup=\"true\" class=\"dropdown-toggle-split dropdown-toggle btn-outline-secondary btn\" data-toggle=\"dropdown\" type=\"button\"><span class=\"sr-only\">Toggle Dropdown</span></button><div class=\"dropdown-menu\"><div>Edit</div><div>Delete</div></div>";
            Assert.AreEqual(expected, output.Content.GetContent());
        }
        [TestMethod()]
        public void ProcessCustomButtonTest()
        {
            var mg = MockHtmlGenerator();
            var viewContext = GetViewContext();
            var Routes = new Dictionary<string, string>();
            Routes.Add("pageSize", "10");
            Routes.Add("id", "1");


            var editTag = new TagBuilder("div");
            editTag.InnerHtml.Append("Edit");
            mg.Setup(x => x.GenerateActionLink(viewContext, "Edit", "Edit", null, null, null, null, Routes, It.IsAny<Object>())).Returns(editTag);
            var deleteTag = new TagBuilder("div");
            deleteTag.InnerHtml.Append("Delete");
            mg.Setup(x => x.GenerateActionLink(viewContext, "Delete", "Delete", null, null, null, null, Routes, It.IsAny<Object>())).Returns(deleteTag);
            var detailsTag = new TagBuilder("div");
            detailsTag.InnerHtml.Append("Details");
            mg.Setup(x => x.GenerateActionLink(viewContext, "Details", "Details", null, null, null, null, Routes, It.IsAny<Object>())).Returns(detailsTag);

            var options = new CoreGridFSPOptions();


            var r = new RowActionTagHelper(mg.Object);
            r.LableName = "TestLabel";
            r.ButtonClass = "btn TestClass btn";//expect it to remove the extra btn
            r.RouteValues = Routes;
            r.Options = options;
            r.ViewContext = viewContext;

            var output = GetTagHelperOutput();

            r.Process(GetTagHelperContext(), output);

            Assert.IsNotNull(output);
            var expected = "<button class=\"TestClass btn\">TestLabel</button><button aria-expanded=\"false\" aria-haspopup=\"true\" class=\"dropdown-toggle-split dropdown-toggle btn-outline-secondary btn\" data-toggle=\"dropdown\" type=\"button\"><span class=\"sr-only\">Toggle Dropdown</span></button><div class=\"dropdown-menu\"><div>Edit</div><div>Delete</div><div>Details</div></div>";
            Assert.AreEqual(expected, output.Content.GetContent());
        }
        public ViewContext GetViewContext()
        {
            var c = new ViewContext();
            return c;
        }
        public TagHelperContext GetTagHelperContext()
        {
            return new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                Guid.NewGuid().ToString("N"));
                
        }
        public TagHelperOutput GetTagHelperOutput()
        {
            var tagHelperOutput = new TagHelperOutput("testOut",
                new TagHelperAttributeList(),
                (result, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetHtmlContent(string.Empty);
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });
            return tagHelperOutput;
        }
        public Mock<IHtmlGenerator> MockHtmlGenerator()
        {
            var gen = new Mock<IHtmlGenerator>();


            return gen;
        }
    }
}