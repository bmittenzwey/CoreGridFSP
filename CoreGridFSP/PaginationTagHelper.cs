using CoreGridFSP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;

namespace CoreGridFSP
{
    public class PaginationTagHelper : TagHelper
    {
        [HtmlAttributeName("CoreGridOptions")]
        public CoreGridFSP.Models.CoreGridFSPOptions Options { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        protected IHtmlGenerator _generator { get; set; }
        public PaginationTagHelper(IHtmlGenerator generator)
        {
            _generator = generator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var options = Options.PaginationOptions;
            Dictionary<string, string> Routes = new Dictionary<string, string>(Options.FilterList);
            if (Options.SelectedSort != null)
                Routes.Add("SelectedSort", Options.SelectedSort);
            Routes.Add("pageSize", options.PageSize.ToString());

            output.TagName = "nav";
            output.Attributes.Add("aria-label", "Page navigation");

            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination flex-wrap");

            ul.InnerHtml.AppendHtml(makeButton("|<", "First", options.EnablePrevious, 1, Routes));
            ul.InnerHtml.AppendHtml(makeButton("<", "Next", options.EnablePrevious, options.CurrentPage - 1, Routes));

            for (var i = 1; i <= options.TotalPages; i++)
            {
                ul.InnerHtml.AppendHtml(makeButton(i.ToString(), null, options.CurrentPage != i, i, Routes));
            }

            ul.InnerHtml.AppendHtml(makeButton(">", "Previous", options.EnableNext, options.CurrentPage + 1, Routes));
            ul.InnerHtml.AppendHtml(makeButton(">|", "Last", options.EnableNext, options.TotalPages, Routes));


            if (options.AllowedPageSizes != null && options.AllowedPageSizes.Length > 0)
            {
                ul.InnerHtml.AppendHtml(makePageSize(Routes));

            }
            output.Content.SetHtmlContent(ul);

        }
        private TagBuilder makePageSize(Dictionary<string, string> Routes)
        {
            var routes = new Dictionary<string, string>(Routes);

            if (routes.ContainsKey("pageSize"))
                routes["pageSize"] = Options.PaginationOptions.PageSize.ToString();
            else
                routes.Add("pageSize", Options.PaginationOptions.PageSize.ToString());

            var li = new TagBuilder("li");
            li.AddCssClass("page-item");

            var btnPgSz = new TagBuilder("button");
            btnPgSz.AddCssClass("btn");
            btnPgSz.AddCssClass("btn-outline-secondary");
            btnPgSz.Attributes.Add("type", "button");
            btnPgSz.InnerHtml.Append("Page Size ");
            int pageSize = Options.PaginationOptions.PageSize;
            btnPgSz.InnerHtml.Append(pageSize==0?"All":pageSize.ToString());
            li.InnerHtml.AppendHtml(btnPgSz);

            var btnDD = new TagBuilder("button");
            btnDD.AddCssClass("btn");
            btnDD.AddCssClass("btn-outline-secondary");
            btnDD.AddCssClass("dropdown-toggle");
            btnDD.AddCssClass("dropdown-toggle-split");
            btnDD.Attributes.Add("type", "button");
            btnDD.Attributes.Add("data-toggle", "dropdown");
            btnDD.Attributes.Add("aria-haspopup", "true");
            btnDD.Attributes.Add("aria-expanded", "false");
            var ddSpan = new TagBuilder("span");
            ddSpan.AddCssClass("sr-only");
            ddSpan.InnerHtml.Append("Toggle Dropdown");
            btnDD.InnerHtml.AppendHtml(ddSpan);
            li.InnerHtml.AppendHtml(btnDD);


            var dropDown = new TagBuilder("div");
            dropDown.AddCssClass("dropdown-menu");

            if (Options.PaginationOptions.AllowAllPageSize)
            {
                routes["pageSize"] = "0";

                var link = _generator.GenerateActionLink(ViewContext, "All", null, null, null, null, null, routes, null);
                link.AddCssClass("dropdown-item");
                link.AddCssClass("page-link");
                link.Attributes.Add("aria-label", "All");
                dropDown.InnerHtml.AppendHtml(link);
            }
            foreach(var size in Options.PaginationOptions.AllowedPageSizes)
            {
                routes["pageSize"] = size.ToString();

                var link = _generator.GenerateActionLink(ViewContext, size.ToString(), null, null, null, null, null, routes, null);
                link.AddCssClass("dropdown-item");
                link.AddCssClass("page-link");
                link.Attributes.Add("aria-label", size.ToString());
                dropDown.InnerHtml.AppendHtml(link);
            }

            li.InnerHtml.AppendHtml(dropDown);
            return li;

        }
        private TagBuilder makeButton(string text, string ariaLabel, bool enabled, int pageNumber, Dictionary<string, string> Routes)
        {
            var routes = new Dictionary<string, string>(Routes);

            if (routes.ContainsKey("currentPage"))
                routes["currentPage"] = pageNumber.ToString();
            else
                routes.Add("currentPage", pageNumber.ToString());


            var li = new TagBuilder("li");
            li.AddCssClass("page-item");


            if (string.IsNullOrEmpty(ariaLabel) && !enabled)
            {
                li.AddCssClass("active");
            }
            else if (!enabled)
                li.AddCssClass("disabled");

            var link = _generator.GenerateActionLink(ViewContext, text, null, null, null, null, null, routes, null);
            link.AddCssClass("page-link");
            if (!string.IsNullOrEmpty(ariaLabel))
            {
                link.InnerHtml.Clear();
                link.Attributes.Add("aria-label", ariaLabel);

                var s1 = new TagBuilder("span");
                s1.Attributes.Add("aria-hidden", "true");
                s1.InnerHtml.Append(text);

                var s2 = new TagBuilder("span");
                s2.AddCssClass("sr-only");
                s2.InnerHtml.Append(ariaLabel);

                link.InnerHtml.AppendHtml(s1);
                link.InnerHtml.AppendHtml(s2);

            }


            li.InnerHtml.AppendHtml(link);
            return li;
        }
        
    }
}
