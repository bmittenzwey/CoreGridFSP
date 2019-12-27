# Index View 

```html
<!-- The model should be updated to use the new wrapper view model-->
@model MvcMovie.Models.MovieGenreViewModel
@{
    ViewData["Title"] = "Index";
}
<!-- CoreGridFSP should be registered -->
@using CoreGridFSP
@addTagHelper *, CoreGridFSP


<script src="~/lib/jquery/dist/jquery.min.js"></script>


<h1>Index</h1>

<p>
    <a asp-action="Create">Create New</a>
</p>


<table id="movies" class="table table-striped table-sm table-hover border-bottom" width="100%" cellspacing="0">
    <thead>
        <tr>
            <th></th>
            <th></th>
            <th>
                <!-- Each sortable or filterable header includes a sortable-header and/or filter-header tag-->
                <sortable-header CoreGridOptions=@Model.CoreGridOptions asp-for="Movies[0].Title"></sortable-header>
                <filter-header asp-for="Movies[0].Title" input-type="TextBox" CoreGridOptions=@Model.CoreGridOptions></filter-header>
            </th>
            <th>
                <sortable-header CoreGridOptions=@Model.CoreGridOptions asp-for="Movies[0].ReleaseDate"></sortable-header>
                <filter-header asp-for="Movies[0].ReleaseDate" input-type="DateTimeRange" CoreGridOptions=@Model.CoreGridOptions></filter-header>
            </th>
            <th>
                <sortable-header CoreGridOptions=@Model.CoreGridOptions asp-for="Movies[0].GenreId"></sortable-header>
                <filter-header asp-for="Movies[0].GenreId" input-type="SelectList" asp-items="Model.Genres" CoreGridOptions=@Model.CoreGridOptions></filter-header>

            </th>
            <th>
                <sortable-header CoreGridOptions=@Model.CoreGridOptions asp-for="Movies[0].Price"></sortable-header>
                <filter-header asp-for="Movies[0].Price" input-type="NumericRange" CoreGridOptions=@Model.CoreGridOptions></filter-header>
            </th>

        </tr>
    </thead>
    <tbody>
        @foreach (var item in Model.Movies)
        {
            <tr>
                <td>
                    <img src="@Html.Raw(item.ImagePath)" class="img-thumbnail rounded" style="max-height:60px;max-width:60px;" />
                </td>
                <td>
                    <div class="btn-group">
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
                </td>
                <td>
                    @Html.DisplayFor(modelItem => item.Title)
                </td>
                <td>
                    @Html.DisplayFor(modelItem => item.ReleaseDate)
                </td>
                <td>
                    @Html.DisplayFor(modelItem => item.Genre.GenreName)
                </td>
                <td>
                    @Html.DisplayFor(modelItem => item.Price)
                </td>

            </tr>
        }
    </tbody>
</table>
<!-- The pagination tag is added immediately after the table close tag-->
<pagination CoreGridOptions=@Model.CoreGridOptions></pagination>
```