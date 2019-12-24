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

            output.Content.SetHtmlContent(ul);

        }
        private TagBuilder makeButton(string text, string ariaLabel, bool enabled, int pageNumber, Dictionary<string, string> routes)
        {
            routes = new Dictionary<string, string>(routes);

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
