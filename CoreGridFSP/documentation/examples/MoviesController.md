This example focuses on the Index GET action.

```csharp
        // GET: Movies
        public async Task<IActionResult> Index(int? currentPage, int? movieGenre, DateTime? ReleaseDate_low, DateTime? ReleaseDate_high, decimal? Price_Low, decimal? Price_High, string title, int? pageSize, string selectedSort = "")
        {
            //Set up the CoreGridFSPOptions to be used for coordinating sorting, filtering and pagination
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

            //Add all filters to the list of routes
            //Select List Example
            if(movieGenre.HasValue)
                gridOptions.FilterList.Add("movieGenre", movieGenre.Value.ToString());
            //Date Range Example
            if(ReleaseDate_low.HasValue)
                gridOptions.FilterList.Add("ReleaseDate_low", ReleaseDate_low.Value.ToString("s"));
            if(ReleaseDate_high.HasValue)
                gridOptions.FilterList.Add("ReleaseDate_high", ReleaseDate_high.Value.ToString("s"));
            //Numeric Range Example
            if(Price_Low.HasValue)  
                gridOptions.FilterList.Add("Price_Low", Price_Low.ToString());
            if(Price_High.HasValue)
                gridOptions.FilterList.Add("Price_High", Price_High.ToString());
            //Simple Text Filter Example
            if(!string.IsNullOrWhiteSpace(title))
                gridOptions.FilterList.Add("title", title);
         

            // Use LINQ to get list of genres.
            IQueryable<Genre> genreQuery = from g in _context.Genre                                          
                                            orderby g.GenreName
                                            select new Genre
                                            {Id = g.Id, 
                                             GenreName = g.GenreName};
           
            //Main Query
            var movies = _context.Movie
                .Include(m => m.Genre)
                .Where(m => string.IsNullOrEmpty(title) || m.Title.Contains(title))
                .Where(m => !movieGenre.HasValue || (m.GenreId == movieGenre.Value))
                .Where(m => !ReleaseDate_low.HasValue || (m.ReleaseDate >= ReleaseDate_low.Value))
                .Where(m => !ReleaseDate_high.HasValue || (m.ReleaseDate <= ReleaseDate_high.Value))
                .Where(m => !Price_Low.HasValue || (m.Price >= Price_Low.Value))
                .Where(m => !Price_High.HasValue || (m.Price <= Price_High.Value))
                ;
            

            // Collect total row count information for pagination
            gridOptions.PaginationOptions.Count = movies.Count();
            var page = (!currentPage.HasValue || currentPage.Value == 0) ? 1 : currentPage.Value;
            gridOptions.PaginationOptions.CurrentPage = page;

            // Add sorting 
            // One case per column that can be sorted
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
            List<Movie> movieList;
            // Perform pagination if the page size is set to a positive number
            if (pageSize.HasValue 
                && pageSize.Value > 0)
            {
                movieList = await movies
                    .Skip((page - 1) * pageSize.Value)
                    .Take(pageSize.Value)
                    .ToListAsync();
            }
            else
                movieList = await movies.ToListAsync();

                

            var movieGenreVM = new MovieGenreViewModel
            {
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync(), "Id", "GenreName"),
                Movies = movieList,
                CoreGridOptions = gridOptions,
            };

            return View(movieGenreVM);
        }
```