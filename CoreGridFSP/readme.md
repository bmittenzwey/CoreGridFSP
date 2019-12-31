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
 var gridOptions = this.HttpContext.Request.ExtractCoreGridOptions();
```

#### Use the parameters or the FilterList to build out EF `where` clauses

#### After filtering the source rows, you can collect some pagination details:
```csharp
gridOptions.PaginationOptions.Count = movies.Count();
gridOptions.PaginationOptions.CurrentPage = (!currentPage.HasValue || currentPage.Value == 0) ? 1 : currentPage.Value;
```

For simple sorting, where the sortable-headers are all pointing to a model property, use the supplied QueryExtensions:
```csharp
using CoreGridFSP.Extensions; 
```

```csharp
if (!String.IsNullOrWhiteSpace(gridOptions.SelectedSortName))
    movies = movies.OrderBy<Movie>(gridOptions.SelectedSortName, 
        gridOptions.SelectedSortDirection == SortableHeaderTagHelper.SortDirection.Desc);

```

Use the SelectedSort parameters to build out the EF `OrderBy` if you need custom sorting.
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

| Tag Helper | Description |
|:--|:--|
| [FilterHeaderTagHelper](https://github.com/bmittenzwey/CoreGridFSP/blob/master/CoreGridFSP/documentation/FilterHeader.md) | Adds a Filter Dropdown to a table header for a specific column. |
| [PaginationTagHelper](https://github.com/bmittenzwey/CoreGridFSP/blob/master/CoreGridFSP/documentation/Pagination.md) | Adds pagination to the bottom of a table.|
| [RowAction](https://github.com/bmittenzwey/CoreGridFSP/blob/master/CoreGridFSP/documentation/RowAction.md) |Makes adding an Action dropdown to a grid simpler.|
| [SortableHeaderTagHelper](https://github.com/bmittenzwey/CoreGridFSP/blob/master/CoreGridFSP/documentation/SortableHeader.md) | Adds sort options to a table header|

| Option Model |
| :--|
| [PaginagionOptions](https://github.com/bmittenzwey/CoreGridFSP/blob/master/CoreGridFSP/documentation/models/PaginationOptions.md)|
| [CoreGridFSPOptions](https://github.com/bmittenzwey/CoreGridFSP/blob/master/CoreGridFSP/documentation/models/CoreGridFSPOptions.md)|

# Extensions
A few extensions are available to make controller-side coding simpler.
To use include the CoreGridFSP.Extensions namespace:
```csharp
using CoreGridFSP.Extensions;
```
| Extension | Desctiption |
| :-- | :-- |
| InheritedModelExtensions | Copies all common properties from one model to another. Useful when creating view models that inherit from data models. |
| QueryExtensions | Allows string column names to be used to sort a data set. |
| ExtractCoreGridOptionsExtensions | Automatically generates a CoreGridFSPOptions object from the HttpContext of an GET Index() controller.|


# Examples
All examples are based on the [MVCMovies tutorial](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-3.1&tabs=visual-studio) provided by Microsoft.

- [Index Get method](https://github.com/bmittenzwey/CoreGridFSP/blob/master/CoreGridFSP/documentation/examples/MoviesController.md)
- [Index View Model](https://github.com/bmittenzwey/CoreGridFSP/blob/master/CoreGridFSP/documentation/examples/IndexViewModel.md)
- [Index View](https://github.com/bmittenzwey/CoreGridFSP/blob/master/CoreGridFSP/documentation/examples/IndexView.md)


# References
- [CoreGridFSP on NuGet](https://www.nuget.org/packages/CoreGridFSP/)
- [CoreGridFSP Source on GitHub](https://github.com/bmittenzwey/CoreGridFSP) 
- [Get Started with ASP.NET Core MVC](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-3.1&tabs=visual-studio)
