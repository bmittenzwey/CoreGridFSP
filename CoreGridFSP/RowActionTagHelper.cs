using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreGridFSP
{
   /// <summary>
   ///  Makes adding an Action dropdown to a grid simpler
   /// </summary>
    public class RowActionTagHelper: TagHelper
    {
        private IDictionary<string, string> _routeValues;

        [HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix = "asp-route-")]
        public IDictionary<string, string> RouteValues
        {
            get
            {
                if (_routeValues == null)
                    _routeValues = (IDictionary<string, string>)new Dictionary<string, string>((IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase);
                return this._routeValues;
            }
            set
            {
                this._routeValues = value;
            }
        }
        [HtmlAttributeName("can-edit")]
        public bool CanEdit { get; set; } = true;
        [HtmlAttributeName("can-delete")]
        public bool CanDelete { get; set; } = true;
        [HtmlAttributeName("can-view-details")]
        public bool CanView { get; set; } = true;
        [HtmlAttributeName("CoreGridOptions")]
        public CoreGridFSP.Models.CoreGridFSPOptions Options { get; set; }
        [HtmlAttributeName("label-name")]
        public string LableName { get; set; } = "Action";
        [HtmlAttributeName("button-class")]
        public string ButtonClass { get; set; } = "btn-outline-secondary btn-sm";

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        protected IHtmlGenerator _generator { get; set; }

        public RowActionTagHelper(IHtmlGenerator generator)
        {
            _generator = generator;
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            Dictionary<string, string> Routes = new Dictionary<string, string>(Options.FilterList);
            if (Options.SelectedSort != null)
                Routes.Add("SelectedSort", Options.SelectedSort);
            Routes.Add("pageSize", Options.PaginationOptions.PageSize.ToString());
            foreach(var route in _routeValues)
                Routes.TryAdd(route.Key, route.Value);


            output.TagName = "div";
            output.Attributes.Add("class", "btn-group");

            var btn = new TagBuilder("button");
            List<string> clist = new List<string>();

            btn.AddCssClass("btn");
            clist.Add("btn");
            
            foreach(var cls in ButtonClass.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                if(!clist.Contains(cls))
                {
                    btn.AddCssClass(cls);
                    clist.Add(cls);
                }
            //btn.AddCssClass("btn-outline-secondary");
            //btn.AddCssClass("btn-sm");
            btn.InnerHtml.Append(LableName);            
            output.Content.AppendHtml(btn);

            var dbtn = new TagBuilder("button");
            dbtn.AddCssClass("btn");
            dbtn.AddCssClass("btn-outline-secondary");
            dbtn.AddCssClass("dropdown-toggle");
            dbtn.AddCssClass("dropdown-toggle-split");
            dbtn.Attributes.Add("type", "button");
            dbtn.Attributes.Add("data-toggle", "dropdown");
            dbtn.Attributes.Add("aria-haspopup", "true");
            dbtn.Attributes.Add("aria-expanded", "false");
            var span = new TagBuilder("span");
            span.AddCssClass("sr-only");
            span.InnerHtml.Append("Toggle Dropdown");
            dbtn.InnerHtml.AppendHtml(span);
            output.Content.AppendHtml(dbtn);

            var menu = new TagBuilder("div");
            menu.AddCssClass("dropdown-menu");
            if(CanEdit)
            {
                var editLink = _generator.GenerateActionLink(ViewContext, "Edit", "Edit", null, null, null, null, Routes, new { @class="dropdown-item" });
                menu.InnerHtml.AppendHtml(editLink);
            }
            if (CanDelete)
            {
                var deleteLink = _generator.GenerateActionLink(ViewContext, "Delete", "Delete", null, null, null, null, Routes, new { @class = "dropdown-item" });
                menu.InnerHtml.AppendHtml(deleteLink);
            }
            if (CanView)
            {
                var detailsLink = _generator.GenerateActionLink(ViewContext, "Details", "Details", null, null, null, null, Routes, new { @class = "dropdown-item" });
                menu.InnerHtml.AppendHtml(detailsLink);
            }


            output.Content.AppendHtml(menu);

        }
    }

    /*
     * <div class="btn-group">
                        <button type="button" class="btn btn-outline-secondary btn-sm">
                            Action
                        </button>
                        <button type="button" class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            <span class="sr-only">Toggle Dropdown</span>
                        </button>
                        <div class="dropdown-menu">
                            <a class="dropdown-item" asp-action="Edit" asp-route-id="@item.Id">Edit</a>
                            <a class="dropdown-item" asp-action="Details" asp-route-id="@item.Id">Details</a>
                            <a class="dropdown-item" asp-action="Delete" asp-route-id="@item.Id">Delete</a>
                        </div>
                    </div>
                    */
}
