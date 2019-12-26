# CoreGridFSP
Helper Tags for use with the standard dotnet core 3.1+ MVC razor grid. Adds Filtering and Sorting to grid headers and makes the standard bootstrap pagination work with the grid filters and sorting.

## Common Requirements
For any page that will use these `TagHelpers`, some common code needs to be added to the View html:

### Register `CoreGridFSP` for use in the View
This will make the `CoreGridFSP` `TagHelpers` available to the View.

``` html
@using CoreGridFSP
@addTagHelper *, CoreGridFSP
```

### Setup Required Objects in the Controller
Like any other Index controller, you will have to add a parameter for the pagination, sort and any filter you are implementing

```csharp
        // GET: Movies
        public async Task<IActionResult> Index(int? currentPage, int? movieGenre, DateTime? fromReleaseDate, DateTime? toReleaseDate, decimal? lowPrice, decimal? highPrice, string searchString, int? pageSize, string selectedSort = "")
        {
```
#### If using the pagination options, be sure to include:
* `int? currentPage`
* `int? pageSize`


#### If using any sorts, be sure to include:
* `string selectedSort = ""`

#### If using any filters, add a parameter for each filtered column.

Read all the parameters from the function signature into a new `CoreGridFSPOptions` object

```csharp
var gridOptions = new CoreGridFSPOptions()
{
    PaginationOptions = new PaginationOptions()
    {
        CurrentPage = currentPage.HasValue?currentPage.Value:1,

    },
    SelectedSort = selectedSort
};
if (pageSize.HasValue)
    gridOptions.PaginationOptions.PageSize = pageSize.Value;
else
    pageSize = gridOptions.PaginationOptions.PageSize;

gridOptions.FilterList.Add("movieGenre", movieGenre.ToString());
gridOptions.FilterList.Add("fromReleaseDate", fromReleaseDate.ToString());
gridOptions.FilterList.Add("toReleaseDate", toReleaseDate.ToString());
gridOptions.FilterList.Add("lowPrice", lowPrice.ToString());
gridOptions.FilterList.Add("highPrice", highPrice.ToString());
gridOptions.FilterList.Add("searchString", searchString);

```

#### Use the parameters or the FilterList to build out EF `where` clauses

#### After filtering the source rows, you can collect some pagination details:
```csharp
gridOptions.PaginationOptions.Count = movies.Count();
gridOptions.PaginationOptions.CurrentPage = (!currentPage.HasValue || currentPage.Value == 0) ? 1 : currentPage.Value;
```

Use the SelectedSort parameters to build out the EF `OrderBy`
```csharp

            switch(gridOptions.SelectedSort == null?"":gridOptions.SelectedSortName.ToUpper())
            {
                case "TITLE":
                    if(gridOptions.SelectedSortDirection== SortableHeaderTagHelper.SortDirection.Asc)
                        movies = movies.OrderBy(m => m.Title);
                    else
                        movies = movies.OrderByDescending(m => m.Title);
                    break;
                case "RELEASEDATE":
                    if(gridOptions.SelectedSortDirection == SortableHeaderTagHelper.SortDirection.Asc)
                        movies = movies.OrderBy(m => m.ReleaseDate);
                    else
                        movies = movies.OrderByDescending(m => m.ReleaseDate);
                    break;
                case "GENREID":
                    if(gridOptions.SelectedSortDirection== SortableHeaderTagHelper.SortDirection.Asc)
                        movies = movies.OrderBy(m => m.Genre.GenreName);
                   else
                       movies = movies.OrderByDescending(m => m.Genre.GenreName);
                    break;
                case "PRICE":
                    if(gridOptions.SelectedSortDirection== SortableHeaderTagHelper.SortDirection.Asc)
                        movies = movies.OrderBy(m => m.Price);
                    else
                        movies = movies.OrderByDescending(m => m.Price);
                    break;
            }
```

Add the CoreGridOptions to your view model and return that to the view


## FilterHeaderTagHelper
Adds a Filter Dropdown to a table header for a specific column.

| Attribute | Description |
|:--|:--|
| asp-for | The name of the filter. For Range filters, this will be used as the low end |
| input-type | TextBox, SelectList, DateRange, NumericRange, Checkbox |
| CoreGridOptions | A CoreGridFSPOptions object that should be passed in through the view model |
| asp-for-end | The high or end range filter for range filters |


## SortableHeaderTagHelper
Adds sort options to a table header

| Attribute | Description |
|:--|:--|
| CoreGridOptions | A CoreGridFSPOptions object that should be passed in through the view model |
| asp-for | The model attribute to sort on |

## PaginationTagHelper
Adds pagination to the bottom of a table.

New CoreGridOptions will set a default page size and set of allowed page sizes. Those can be overridden if needed.

| Attribute | Description |
|:--|:--|
| CoreGridOptions | A CoreGridFSPOptions object that should be passed in through the view model |

### PaginagionOptions

| Attribute | Description |
|:--|:--|
| PageSize | The number of rows to display on a page. A value of 0 should be interpreted as display all rows |
| CurrentPage | The current page being displayed |
| Count | The number of total results matching any current filters. Should be set in the controller. |
| TotalPages | Read-Only calculation of the total number of pages based on page size and count |
| EnablePrevious | Read-Only Based on the current page |
| EnableNext | Read-Only Based on the current page and total pages |
| AllowAllPageSize | Include the "All" page in the page size select list |
| AllowedPageSizes | An int array list of all allowed page sizes |

## CoreGridFSPOptions

| Attribute | Description |
|:--|:--|
|PaginationOptions | A `PaginationOptions` object that provides all values needed for pagination |
| SelectedSort | Calculated combination of Name and Direction |
| SelectedSortName | The name of the column to sort on |
| SelectedSortDirection | Asc or Desc |
| FilterList | Dictionary kvp list with all current filter values | 