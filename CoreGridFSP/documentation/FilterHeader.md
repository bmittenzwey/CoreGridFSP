# FilterHeaderTagHelper
Adds a Filter Dropdown to a table header for a specific column.

| Attribute | Description |
|:--|:--|
| asp-for | The property in a data model to filter on |
| input-type | TextBox, SelectList, DateRange, DateTimeRange, NumericRange, Checkbox |
| CoreGridOptions | A CoreGridFSPOptions object that should be passed in through the view model |
| asp-items | A kvp select list object with keys and values to display in the dropdown list |
| high-range-suffix | Used for range filters. The high range suffix is added to the column name. Defaults to "_high" |
| low-range-suffix | Used for range filters. The low range suffix is added to the column name. Defaults to "_low" |
