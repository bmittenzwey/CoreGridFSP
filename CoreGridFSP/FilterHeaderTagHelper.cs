using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreGridFSP
{
    public class FilterHeaderTagHelper : TagHelper
    {
        public enum InputType
        {
            TextBox,
            SelectList,
            CheckBox,
            DateRange,
            NumericRange
        }
        [HtmlAttributeName("asp-for")]
        public ModelExpression aspFor { get; set; }
        [HtmlAttributeName("asp-for-end")]
        public ModelExpression aspForEnd { get; set; }
        [HtmlAttributeName("input-type")]
        public InputType inputType { get; set; }

        [HtmlAttributeName("asp-items")]
        public SelectList aspItems { get; set; }
        [HtmlAttributeName("CoreGridOptions")]
        public CoreGridFSP.Models.CoreGridFSPOptions Options { get; set; }


        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        protected IHtmlGenerator _generator { get; set; }

        public FilterHeaderTagHelper(IHtmlGenerator generator)
        {
            _generator = generator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            Dictionary<string, string> Routes = new Dictionary<string, string>(Options.FilterList);
            if (Options.SelectedSort != null)
                Routes.Add("SelectedSort", Options.SelectedSort);

            output.TagName = "div";

            output.Attributes.Add("class", "btn-group btn-group-sm");
            //Toggle Button
            var btn = new TagBuilder("button");
            btn.AddCssClass("dropdown-toggle");
            btn.AddCssClass("btn-outline-secondary");
            btn.AddCssClass("btn-sm");
            btn.AddCssClass("btn");

            btn.Attributes.Add("data-toggle", "dropdown");
            btn.Attributes.Add("data-display", "static");
            btn.Attributes.Add("type", "button");
            btn.Attributes.Add("aria-haspopups", "true");
            btn.Attributes.Add("aria-expanded", "false");

            var img = new TagBuilder("img");
            img.Attributes.Add("src", "images/filter.png");
            img.Attributes.Add("style", "max-height:20px;");
            img.TagRenderMode = TagRenderMode.SelfClosing;
            btn.InnerHtml.AppendHtml(img);
            output.Content.AppendHtml(btn);

            // Dropdown Menu
            var dropdown = new TagBuilder("div");
            dropdown.AddCssClass("keep-open");
            dropdown.AddCssClass("dropdown-menu");
            dropdown.Attributes.Add("onclick", "function keepOpen(){event.stopPropagation();};keepOpen();");
            var form = new TagBuilder("form");
            form.AddCssClass("py-3");
            form.AddCssClass("px-4");



            foreach (var route in Routes)
            {


                if ((aspFor == null || route.Key.ToUpper() != aspFor.Name.ToUpper())
                    && route.Key.ToLower() != "currentpage"
                    && (aspForEnd == null || route.Key.ToUpper() != aspForEnd.Name.ToUpper()))
                {
                    var hidden = new TagBuilder("input");
                    hidden.TagRenderMode = TagRenderMode.SelfClosing;
                    hidden.Attributes.Add("value", route.Value);
                    hidden.Attributes.Add("type", "hidden");
                    hidden.Attributes.Add("data-val", "true");

                    hidden.Attributes.Add("name", route.Key);
                    hidden.Attributes.Add("id", route.Key);

                    form.InnerHtml.AppendHtml(hidden);

                }
            }

            switch (this.inputType)
            {
                case InputType.TextBox:
                    var form_group = new TagBuilder("div");
                    form_group.AddCssClass("form-group");

                    var label = _generator.GenerateLabel(ViewContext, aspFor.ModelExplorer, aspFor.Name, null, new { @class = "control-label" });
                    form_group.InnerHtml.AppendHtml(label);
                    var input = _generator.GenerateTextBox(ViewContext, aspFor.ModelExplorer, aspFor.Name, null, null, new { @class = "form-control" });
                    input.TagRenderMode = TagRenderMode.SelfClosing;

                    form_group.InnerHtml.AppendHtml(input);
                    form.InnerHtml.AppendHtml(form_group);

                    break;
                case InputType.SelectList:
                    var sform_group = new TagBuilder("div");
                    sform_group.AddCssClass("form-group");

                    var slabel = _generator.GenerateLabel(ViewContext, aspFor.ModelExplorer, aspFor.Name, null, new { @class = "control-label" });
                    sform_group.InnerHtml.AppendHtml(slabel);

                    var select = _generator.GenerateSelect(ViewContext, aspFor.ModelExplorer, "All", aspFor.Name, aspItems, false, null);
                    select.AddCssClass("form-control");
                    select.Attributes.Add("data-role", "select-dropdown");
                    sform_group.InnerHtml.AppendHtml(select);

                    form.InnerHtml.AppendHtml(sform_group);
                    break;
                case InputType.CheckBox:
                    var chk_form_group = new TagBuilder("div");
                    chk_form_group.AddCssClass("form-group");
                    bool isChecked = false;
                    if (Routes.ContainsKey(aspFor.Name))
                        isChecked = bool.Parse(Routes[aspFor.Name]);
                    var chklabel = _generator.GenerateLabel(ViewContext, aspFor.ModelExplorer, aspFor.Name, null, new { @class = "control-label" });
                    chk_form_group.InnerHtml.AppendHtml(chklabel);
                    var chk = _generator.GenerateCheckBox(ViewContext, aspFor.ModelExplorer, null, isChecked, new { @class = "control-label" });

                    form.InnerHtml.AppendHtml(chk_form_group);
                    break;
                case InputType.DateRange:
                    var from_form_group = new TagBuilder("div");
                    from_form_group.AddCssClass("form-group");

                    from_form_group.InnerHtml.Append("From Date");
                    var from = _generator.GenerateTextBox(ViewContext, aspFor.ModelExplorer, aspFor.Name, null, null, new { @class = "form-control" });
                    from.TagRenderMode = TagRenderMode.SelfClosing;

                    if (from.Attributes.ContainsKey("type"))
                        from.Attributes["type"] = "date";
                    else
                        from.Attributes.Add("type", "date");
                    from_form_group.InnerHtml.AppendHtml(from);

                    var toFG = new TagBuilder("div");
                    toFG.AddCssClass("form-group");
                    toFG.InnerHtml.Append("To Date");
                    var to = _generator.GenerateTextBox(ViewContext, aspForEnd.ModelExplorer, aspForEnd.Name, null, null, new { @class = "form-control" });
                    to.TagRenderMode = TagRenderMode.SelfClosing;
                    if (to.Attributes.ContainsKey("type"))
                        to.Attributes["type"] = "date";
                    else
                        to.Attributes.Add("type", "date");

                    toFG.InnerHtml.AppendHtml(to);
                    form.InnerHtml.AppendHtml(from_form_group);
                    form.InnerHtml.AppendHtml(toFG);
                    break;

                case InputType.NumericRange:
                    var low_form_group = new TagBuilder("div");
                    low_form_group.AddCssClass("form-group");

                    low_form_group.InnerHtml.Append("From Value");
                    var low = _generator.GenerateTextBox(ViewContext, aspFor.ModelExplorer, aspFor.Name, null, null, new { @class = "form-control" });
                    low.TagRenderMode = TagRenderMode.SelfClosing;
                    if (low.Attributes.ContainsKey("type"))
                        low.Attributes["type"] = "number";
                    else
                        low.Attributes.Add("type", "number");
                    low_form_group.InnerHtml.AppendHtml(low);

                    var highFG = new TagBuilder("div");
                    highFG.AddCssClass("form-group");
                    highFG.InnerHtml.Append("To Value");
                    var high = _generator.GenerateTextBox(ViewContext, aspForEnd.ModelExplorer, aspForEnd.Name, null, null, new { @class = "form-control" });
                    high.TagRenderMode = TagRenderMode.SelfClosing;
                    if (high.Attributes.ContainsKey("type"))
                        high.Attributes["type"] = "number";
                    else
                        high.Attributes.Add("type", "number");

                    highFG.InnerHtml.AppendHtml(high);

                    var hValid = _generator.GenerateValidationMessage(ViewContext, aspForEnd.ModelExplorer, aspForEnd.Name, null, null, new { @class = "text-danger" });
                    highFG.InnerHtml.AppendHtml(hValid);
                    form.InnerHtml.AppendHtml(low_form_group);
                    form.InnerHtml.AppendHtml(highFG);
                    break;
            }


            var submit = new TagBuilder("button");
            submit.AddCssClass("btn-outline-secondary");
            submit.AddCssClass("btn");
            submit.Attributes.Add("type", "submit");
            submit.InnerHtml.Append("Filter");
            form.InnerHtml.AppendHtml(submit);




            //var script = new TagBuilder("script");
            //script.Attributes.Add("type", "text/javascript");
            //script.Attributes.Add("language", "javascript");
            //script.InnerHtml.AppendHtml("$('div.keep-open').on('click', function (e) {event.stopPropogation();});");
            //dropdown.InnerHtml.AppendHtml(script);
            dropdown.InnerHtml.AppendHtml(form);
            output.Content.AppendHtml(dropdown);




        }

    }
}
