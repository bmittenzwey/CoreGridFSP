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
        public async Task<IActionResult> Index(int? currentPage, int? movieGenre, DateTime? fromReleaseDate, DateTime? toReleaseDate, decimal? lowPrice, decimal? highPrice, string searchString, string selectedSort = "")
        {
```

Read all the parameters from the function signature into a new `CoreGridFSPOptions` object

```csharp
var pageSize = 4;
var gridOptions = new CoreGridFSPOptions()
{
    PaginationOptions = new PaginationOptions()
    {
        PageSize = pageSize,
        CurrentPage = currentPage.HasValue?currentPage.Value:1,

    },
    SelectedSort = selectedSort
};
gridOptions.FilterList.Add("movieGenre", movieGenre.ToString());
gridOptions.FilterList.Add("fromReleaseDate", fromReleaseDate.ToString());
gridOptions.FilterList.Add("toReleaseDate", toReleaseDate.ToString());
gridOptions.FilterList.Add("lowPrice", lowPrice.ToString());
gridOptions.FilterList.Add("highPrice", highPrice.ToString());
gridOptions.FilterList.Add("searchString", searchString);
```

Use the parameters or the FilterList to build out EF `where` clauses
After filtering the source rows, you can collect some pagination details:
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
Adds a Filter Dropdown to a table header

## SortableHeaderTagHelper
Adds sort options to a table header

## PaginationTagHelper
Adds pagination to the bottom of a table

