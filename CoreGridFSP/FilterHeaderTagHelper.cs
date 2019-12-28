using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

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
            DateTimeRange,
            NumericRange
        }

        [HtmlAttributeName("asp-for")]
        public ModelExpression aspFor { get; set; }
        [HtmlAttributeName("input-type")]
        public InputType inputType { get; set; }

        [HtmlAttributeName("asp-items")]
        public SelectList aspItems { get; set; }
        [HtmlAttributeName("CoreGridOptions")]
        public CoreGridFSP.Models.CoreGridFSPOptions Options { get; set; }
        [HtmlAttributeName("low-range-suffix")]
        public string lowRangeSuffix { get; set; } = "_low";
        [HtmlAttributeName("high-range-suffix")]
        public string highRangeSuffix { get; set; } = "_high";

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
            var displayName = aspFor.Metadata.DisplayName ?? aspFor.Metadata.Name;
            var filterNames = new List<string>();
            if (this.inputType == InputType.CheckBox || this.inputType == InputType.SelectList || this.inputType == InputType.TextBox)
            {
                if (aspFor != null)
                    filterNames.Add(aspFor.Metadata.Name);
            }
            else
            {
                if (aspFor != null)
                {
                    filterNames.Add($"{aspFor.Metadata.Name.Trim()}{this.lowRangeSuffix}");
                    filterNames.Add($"{aspFor.Metadata.Name.Trim()}{this.highRangeSuffix}");
                }
            }
            foreach(var filter in filterNames)
            {
                if (!Options.FilterList.ContainsKey(filter.Trim()))
                    Options.FilterList.Add(filter.Trim(), "");
            }

            Dictionary<string, string> Routes = new Dictionary<string, string>(Options.FilterList);
            if (Options.SelectedSort != null)
                Routes.Add("SelectedSort", Options.SelectedSort);
            Routes.Add("pageSize", Options.PaginationOptions.PageSize.ToString());
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
                

                if (!filterNames.Any(f => f.ToUpper()==route.Key.ToUpper())
                    && route.Key.ToLower() != "currentpage")
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

                    //var label = _generator.GenerateLabel(ViewContext, aspFor.ModelExplorer, aspFor.Name, null, new { @class = "control-label" });
                    var label = new TagBuilder("label");
                    label.AddCssClass("control-label");
                    label.Attributes.Add("for", aspFor.Metadata.Name);
                    label.InnerHtml.Append(displayName);

                    form_group.InnerHtml.AppendHtml(label);
                    //var input = _generator.GenerateTextBox(ViewContext, aspFor.ModelExplorer, aspFor.Name, null, null, new { @class = "form-control" });
                    var input = new TagBuilder("input");
                    input.AddCssClass("form-control");
                    input.Attributes.Add("id", aspFor.Metadata.Name);
                    input.Attributes.Add("name", aspFor.Metadata.Name);
                    input.Attributes.Add("type", "text");
                    input.Attributes.Add("value", Options.FilterList[aspFor.Metadata.Name.Trim()]);
                    input.TagRenderMode = TagRenderMode.SelfClosing;

                    form_group.InnerHtml.AppendHtml(input);
                    form.InnerHtml.AppendHtml(form_group);

                    break;
                case InputType.SelectList:
                    var sform_group = new TagBuilder("div");
                    sform_group.AddCssClass("form-group");

                    var slabel = _generator.GenerateLabel(ViewContext, aspFor.ModelExplorer, aspFor.Metadata.Name, null, new { @class = "control-label" });
                    sform_group.InnerHtml.AppendHtml(slabel);

                    var select = new TagBuilder("select");
                    select.AddCssClass("form-control");
                    select.Attributes.Add("name", displayName);
                    select.Attributes.Add("id", displayName);
                    select.Attributes.Add("data-role", "select-dropdown");
                    
                    var aoption = new TagBuilder("option");
                    aoption.Attributes.Add("value", "");
                    aoption.InnerHtml.Append("All");

                    select.InnerHtml.AppendHtml(aoption);
                    foreach(var item in aspItems)
                    {
                        var option = new TagBuilder("option");
                        option.Attributes.Add("value", item.Value);
                        option.InnerHtml.Append(item.Text);
                        if (item.Selected || item.Value.Trim() == Options.FilterList[aspFor.Metadata.Name.Trim()])
                        {
                            option.Attributes.Add("selected", "selected");
                        }

                        select.InnerHtml.AppendHtml(option);
                    }

                    //var select = _generator.GenerateSelect(ViewContext, aspFor.ModelExplorer, "All", aspFor.Metadata.Name, aspItems, false, null);
                    
                    sform_group.InnerHtml.AppendHtml(select);

                    form.InnerHtml.AppendHtml(sform_group);
                    break;
                case InputType.CheckBox:
                    var chk_form_group = new TagBuilder("div");
                    chk_form_group.AddCssClass("form-group");
                    bool isChecked = false;
                    if (Routes.ContainsKey(aspFor.Metadata.Name))
                        isChecked = bool.Parse(Routes[aspFor.Metadata.Name]);
                    var chklabel = _generator.GenerateLabel(ViewContext, aspFor.ModelExplorer, displayName, null, new { @class = "control-label" });
                    chk_form_group.InnerHtml.AppendHtml(chklabel);
                    var chk = _generator.GenerateCheckBox(ViewContext, aspFor.ModelExplorer, null, isChecked, new { @class = "control-label" });

                    form.InnerHtml.AppendHtml(chk_form_group);
                    break;
                case InputType.DateRange:
                case InputType.DateTimeRange:
                    string inputType = "date";
                    string dateFormat = "yyyy-MM-dd";
                    if (this.inputType == InputType.DateTimeRange)
                    {
                        inputType = "datetime-local";
                        dateFormat = "s";
                    }
                    var from_form_group = new TagBuilder("div");
                    from_form_group.AddCssClass("form-group");

                    var flabel = new TagBuilder("label");
                    flabel.AddCssClass("control-label");
                    flabel.Attributes.Add("for", aspFor.Metadata.Name.Trim()+this.lowRangeSuffix);
                    flabel.InnerHtml.Append($"From {displayName}");

                    from_form_group.InnerHtml.AppendHtml(flabel);

                    //var input = _generator.GenerateTextBox(ViewContext, aspFor.ModelExplorer, aspFor.Name, null, null, new { @class = "form-control" });
                    var from = new TagBuilder("input");
                    from.AddCssClass("form-control");
                    from.Attributes.Add("id", aspFor.Metadata.Name.Trim() + this.lowRangeSuffix);
                    from.Attributes.Add("name", aspFor.Metadata.Name.Trim() + this.lowRangeSuffix);
                    from.Attributes.Add("type", inputType);
                    DateTime lowDate;
                    if( DateTime.TryParse(Options.FilterList[aspFor.Metadata.Name.Trim() + this.lowRangeSuffix], out lowDate))
                        from.Attributes.Add("value", lowDate.ToString(dateFormat ));
                    from.TagRenderMode = TagRenderMode.SelfClosing;


                    //var from = _generator.GenerateTextBox(ViewContext, aspFor.ModelExplorer, aspFor.Name, null, null, new { @class = "form-control" });
                    //from.TagRenderMode = TagRenderMode.SelfClosing;

                    
                    from_form_group.InnerHtml.AppendHtml(from);


                    var toFG = new TagBuilder("div");
                    toFG.AddCssClass("form-group");

                    var tolabel = new TagBuilder("label");
                    tolabel.AddCssClass("control-label");
                    tolabel.Attributes.Add("for", aspFor.Metadata.Name.Trim() + this.highRangeSuffix);
                    tolabel.InnerHtml.Append($"To {displayName}");

                    toFG.InnerHtml.AppendHtml(tolabel);

                    //var input = _generator.GenerateTextBox(ViewContext, aspFor.ModelExplorer, aspFor.Name, null, null, new { @class = "form-control" });
                    var to = new TagBuilder("input");
                    to.AddCssClass("form-control");
                    to.Attributes.Add("id", aspFor.Metadata.Name.Trim() + this.highRangeSuffix);
                    to.Attributes.Add("name", aspFor.Metadata.Name.Trim() + this.highRangeSuffix);
                    to.Attributes.Add("type", inputType);
                    DateTime highDate;
                    if (DateTime.TryParse(Options.FilterList[aspFor.Metadata.Name.Trim() + this.highRangeSuffix], out highDate))
                        to.Attributes.Add("value", highDate.ToString(dateFormat));
                    to.TagRenderMode = TagRenderMode.SelfClosing;


                    //var to = _generator.GenerateTextBox(ViewContext, aspForEnd.ModelExplorer, aspForEnd.Name, null, null, new { @class = "form-control" });
                    to.TagRenderMode = TagRenderMode.SelfClosing;

                    toFG.InnerHtml.AppendHtml(to);
                    form.InnerHtml.AppendHtml(from_form_group);
                    form.InnerHtml.AppendHtml(toFG);
                    break;

                case InputType.NumericRange:
                    var low_form_group = new TagBuilder("div");
                    low_form_group.AddCssClass("form-group");

                    low_form_group.InnerHtml.Append($"From {displayName}");
                    var low = _generator.GenerateTextBox(ViewContext, aspFor.ModelExplorer, aspFor.Metadata.Name.Trim()+lowRangeSuffix, null, null, new { @class = "form-control" });
                    low.TagRenderMode = TagRenderMode.SelfClosing;
                    if (low.Attributes.ContainsKey("type"))
                        low.Attributes["type"] = "number";
                    else
                        low.Attributes.Add("type", "number");
                    low_form_group.InnerHtml.AppendHtml(low);

                    var highFG = new TagBuilder("div");
                    highFG.AddCssClass("form-group");
                    highFG.InnerHtml.Append($"To {displayName}");


                    var high = _generator.GenerateTextBox(ViewContext, aspFor.ModelExplorer, aspFor.Metadata.Name.Trim()+highRangeSuffix, null, null, new { @class = "form-control" });
                    high.TagRenderMode = TagRenderMode.SelfClosing;
                    if (high.Attributes.ContainsKey("type"))
                        high.Attributes["type"] = "number";
                    else
                        high.Attributes.Add("type", "number");

                    highFG.InnerHtml.AppendHtml(high);

                    //var hValid = _generator.GenerateValidationMessage(ViewContext, aspFor.ModelExplorer, aspFor.Metadata.Name, null, null, new { @class = "text-danger" });
                    //highFG.InnerHtml.AppendHtml(hValid);
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

            dropdown.InnerHtml.AppendHtml(form);
            output.Content.AppendHtml(dropdown);




        }

    }
}
