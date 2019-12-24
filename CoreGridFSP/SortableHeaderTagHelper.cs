using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;

namespace CoreGridFSP
{
    public class SortableHeaderTagHelper : TagHelper
    {
        [HtmlAttributeName("CoreGridOptions")]
        public CoreGridFSP.Models.CoreGridFSPOptions Options { get; set; }

        [HtmlAttributeName("asp-for")]
        public ModelExpression aspFor { get; set; }
        
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        protected IHtmlGenerator _generator { get; set; }

        public SortableHeaderTagHelper(IHtmlGenerator generator)
        {
            _generator = generator;
        }
        private const string DOWN_ARROW = "&#x25B2;";
        private const string UP_ARROW = "&#x25BC;";
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //Make a copy of the routes so the changed values won't make it back out to the host page
            IDictionary<string, string> routes = new Dictionary<string, string>(Options.FilterList);
            
            var propName = aspFor.Metadata.Name;
            var heading = aspFor.Metadata.DisplayName ?? propName;

            output.TagName = "div";
            output.Attributes.Add("class", "btn-group");

            output.Content.SetContent(heading);

           
            var selectedSort = Options.SelectedSort;

            if(Options.SelectedSortName != null
                && Options.SelectedSortName.ToUpper() == propName.ToUpper()
                && Options.SelectedSortDirection == SortDirection.Asc)
            {

                output.Content.AppendHtml($"<a class=\"btn btn-primary btn-sm disabled\">{DOWN_ARROW}</a> ");
            }
            else
            {
                routes["SelectedSort"] = $"{propName}Asc";
                routes["currentPage"] = "1";

                var link = _generator.GenerateActionLink(ViewContext, DOWN_ARROW, null, null, null, null, null, routes, null);
                link.InnerHtml.Clear();
                link.InnerHtml.AppendHtml(DOWN_ARROW);
                link.AddCssClass("btn");
                link.AddCssClass("btn-light");
                link.AddCssClass("btn-sm");
                output.Content.AppendHtml(link);
            }

            if (Options.SelectedSortName != null
                && Options.SelectedSortName.ToUpper() == propName.ToUpper()
                && Options.SelectedSortDirection == SortDirection.Desc)
            {

                output.Content.AppendHtml($"<a class=\"btn btn-primary btn-sm disabled\">{UP_ARROW}</a> ");
            }
            else
            {
                routes["SelectedSort"] = $"{propName}Desc";
                routes["currentPage"] = "1";
                var link = _generator.GenerateActionLink(ViewContext, UP_ARROW, null, null, null, null, null, routes, null);
                link.InnerHtml.Clear();
                link.InnerHtml.AppendHtml(UP_ARROW);
                link.AddCssClass("btn");
                link.AddCssClass("btn-light");
                link.AddCssClass("btn-sm");
                output.Content.AppendHtml(link);
            }

        }
        public enum SortDirection
        {
            Asc,
            Desc
        }
    }
}
