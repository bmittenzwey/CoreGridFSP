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
    public class PaginationTagHelperTests
    {
        [TestMethod()]
        public void PaginationTagHelperTest()
        {
            var mg = MockHtmlGenerator();
            var p = new PaginationTagHelper(mg.Object);
            p.Options = new CoreGridFSPOptions();

            Assert.IsNotNull(p.Options);
            Assert.IsTrue(p.Options.PaginationOptions.AllowAllPageSize);
            Assert.AreEqual(5, p.Options.PaginationOptions.AllowedPageSizes.Length);
            Assert.AreEqual(0, p.Options.PaginationOptions.Count);
            Assert.AreEqual(10, p.Options.PaginationOptions.PageSize);
                       
        }

        [TestMethod()]
        public void ProcessDefaultTest()
        {
            var mg = MockHtmlGenerator();
            var viewContext = GetViewContext();
            var Routes = new Dictionary<string, string>();



            mg.Setup(x => x.GenerateActionLink(viewContext, It.IsAny<string>(), null, null, null, null, null, It.IsAny<Dictionary<string, string>>(),  It.IsAny<Object>()))
                .Returns((ViewContext v, string text, string a, string c, string p, string h, string pr, Dictionary<string, string> routes, object attr) => {
                    var tag = new TagBuilder("div");
                    tag.InnerHtml.Append(text);
                    return tag;
                    });
            
            var options = new CoreGridFSPOptions();


            var p = new PaginationTagHelper(mg.Object);
            p.Options = options;

            p.ViewContext = viewContext;

            var output = GetTagHelperOutput();

            p.Process(GetTagHelperContext(), output);

            Assert.IsNotNull(output);
            var expected = "<ul class=\"pagination flex-wrap\"><li class=\"disabled page-item\"><div aria-label=\"First\" class=\"page-link\"><span aria-hidden=\"true\">|&lt;</span><span class=\"sr-only\">First</span></div></li><li class=\"disabled page-item\"><div aria-label=\"Next\" class=\"page-link\"><span aria-hidden=\"true\">&lt;</span><span class=\"sr-only\">Next</span></div></li><li class=\"disabled page-item\"><div aria-label=\"Previous\" class=\"page-link\"><span aria-hidden=\"true\">&gt;</span><span class=\"sr-only\">Previous</span></div></li><li class=\"disabled page-item\"><div aria-label=\"Last\" class=\"page-link\"><span aria-hidden=\"true\">&gt;|</span><span class=\"sr-only\">Last</span></div></li><li class=\"page-item\"><button class=\"btn-outline-secondary btn\" type=\"button\">Page Size 10</button><button aria-expanded=\"false\" aria-haspopup=\"true\" class=\"dropdown-toggle-split dropdown-toggle btn-outline-secondary btn\" data-toggle=\"dropdown\" type=\"button\"><span class=\"sr-only\">Toggle Dropdown</span></button><div class=\"dropdown-menu\"><div aria-label=\"All\" class=\"page-link dropdown-item\">All</div><div aria-label=\"5\" class=\"page-link dropdown-item\">5</div><div aria-label=\"10\" class=\"page-link dropdown-item\">10</div><div aria-label=\"20\" class=\"page-link dropdown-item\">20</div><div aria-label=\"50\" class=\"page-link dropdown-item\">50</div><div aria-label=\"100\" class=\"page-link dropdown-item\">100</div></div></li></ul>";
            Assert.AreEqual(expected, output.Content.GetContent());
        }
          [TestMethod()]
        public void ProcessOnePageTest()
        {
            var mg = MockHtmlGenerator();
            var viewContext = GetViewContext();
            var Routes = new Dictionary<string, string>();



            mg.Setup(x => x.GenerateActionLink(viewContext, It.IsAny<string>(), null, null, null, null, null, It.IsAny<Dictionary<string, string>>(),  It.IsAny<Object>()))
                .Returns((ViewContext v, string text, string a, string c, string p, string h, string pr, Dictionary<string, string> routes, object attr) => {
                    var tag = new TagBuilder("div");
                    tag.InnerHtml.Append(text);
                    return tag;
                    });
            
            var options = new CoreGridFSPOptions();
            options.PaginationOptions.Count = 5;

            var p = new PaginationTagHelper(mg.Object);
            p.Options = options;

            p.ViewContext = viewContext;

            var output = GetTagHelperOutput();

            p.Process(GetTagHelperContext(), output);

            Assert.IsNotNull(output);
            var expected = "<ul class=\"pagination flex-wrap\"><li class=\"disabled page-item\"><div aria-label=\"First\" class=\"page-link\"><span aria-hidden=\"true\">|&lt;</span><span class=\"sr-only\">First</span></div></li><li class=\"disabled page-item\"><div aria-label=\"Next\" class=\"page-link\"><span aria-hidden=\"true\">&lt;</span><span class=\"sr-only\">Next</span></div></li><li class=\"active page-item\"><div class=\"page-link\">1</div></li><li class=\"disabled page-item\"><div aria-label=\"Previous\" class=\"page-link\"><span aria-hidden=\"true\">&gt;</span><span class=\"sr-only\">Previous</span></div></li><li class=\"disabled page-item\"><div aria-label=\"Last\" class=\"page-link\"><span aria-hidden=\"true\">&gt;|</span><span class=\"sr-only\">Last</span></div></li><li class=\"page-item\"><button class=\"btn-outline-secondary btn\" type=\"button\">Page Size 10</button><button aria-expanded=\"false\" aria-haspopup=\"true\" class=\"dropdown-toggle-split dropdown-toggle btn-outline-secondary btn\" data-toggle=\"dropdown\" type=\"button\"><span class=\"sr-only\">Toggle Dropdown</span></button><div class=\"dropdown-menu\"><div aria-label=\"All\" class=\"page-link dropdown-item\">All</div><div aria-label=\"5\" class=\"page-link dropdown-item\">5</div><div aria-label=\"10\" class=\"page-link dropdown-item\">10</div><div aria-label=\"20\" class=\"page-link dropdown-item\">20</div><div aria-label=\"50\" class=\"page-link dropdown-item\">50</div><div aria-label=\"100\" class=\"page-link dropdown-item\">100</div></div></li></ul>";
            Assert.AreEqual(expected, output.Content.GetContent());
        }  
          [TestMethod()]
        public void ProcessPage2Test()
        {
            var mg = MockHtmlGenerator();
            var viewContext = GetViewContext();
            var Routes = new Dictionary<string, string>();



            mg.Setup(x => x.GenerateActionLink(viewContext, It.IsAny<string>(), null, null, null, null, null, It.IsAny<Dictionary<string, string>>(),  It.IsAny<Object>()))
                .Returns((ViewContext v, string text, string a, string c, string p, string h, string pr, Dictionary<string, string> routes, object attr) => {
                    var tag = new TagBuilder("div");
                    tag.InnerHtml.Append(text);
                    return tag;
                    });
            
            var options = new CoreGridFSPOptions();
            options.PaginationOptions.Count = 35;
            options.PaginationOptions.CurrentPage = 2;

            var p = new PaginationTagHelper(mg.Object);
            p.Options = options;

            p.ViewContext = viewContext;

            var output = GetTagHelperOutput();

            p.Process(GetTagHelperContext(), output);

            Assert.IsNotNull(output);
            var expected = "<ul class=\"pagination flex-wrap\"><li class=\"page-item\"><div aria-label=\"First\" class=\"page-link\"><span aria-hidden=\"true\">|&lt;</span><span class=\"sr-only\">First</span></div></li><li class=\"page-item\"><div aria-label=\"Next\" class=\"page-link\"><span aria-hidden=\"true\">&lt;</span><span class=\"sr-only\">Next</span></div></li><li class=\"page-item\"><div class=\"page-link\">1</div></li><li class=\"active page-item\"><div class=\"page-link\">2</div></li><li class=\"page-item\"><div class=\"page-link\">3</div></li><li class=\"page-item\"><div class=\"page-link\">4</div></li><li class=\"page-item\"><div aria-label=\"Previous\" class=\"page-link\"><span aria-hidden=\"true\">&gt;</span><span class=\"sr-only\">Previous</span></div></li><li class=\"page-item\"><div aria-label=\"Last\" class=\"page-link\"><span aria-hidden=\"true\">&gt;|</span><span class=\"sr-only\">Last</span></div></li><li class=\"page-item\"><button class=\"btn-outline-secondary btn\" type=\"button\">Page Size 10</button><button aria-expanded=\"false\" aria-haspopup=\"true\" class=\"dropdown-toggle-split dropdown-toggle btn-outline-secondary btn\" data-toggle=\"dropdown\" type=\"button\"><span class=\"sr-only\">Toggle Dropdown</span></button><div class=\"dropdown-menu\"><div aria-label=\"All\" class=\"page-link dropdown-item\">All</div><div aria-label=\"5\" class=\"page-link dropdown-item\">5</div><div aria-label=\"10\" class=\"page-link dropdown-item\">10</div><div aria-label=\"20\" class=\"page-link dropdown-item\">20</div><div aria-label=\"50\" class=\"page-link dropdown-item\">50</div><div aria-label=\"100\" class=\"page-link dropdown-item\">100</div></div></li></ul>";
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