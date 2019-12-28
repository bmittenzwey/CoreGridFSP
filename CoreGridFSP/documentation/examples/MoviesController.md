This example focuses on the Index GET action.

```csharp
        // GET: Movies
        public async Task<IActionResult> Index(int? currentPage, int? movieGenre, DateTime? ReleaseDate_low, DateTime? ReleaseDate_high, decimal? Price_Low, decimal? Price_High, string title, int? pageSize, string selectedSort = "")
        {
            //Set up the CoreGridFSPOptions to be used for coordinating sorting, filtering and pagination
            var gridOptions = this.HttpContext.Request.ExtractCoreGridOptions();
            
            // Use LINQ to get list of genres.
            IQueryable<Genre> genreQuery = from g in _context.Genre
                                           orderby g.GenreName
                                           select new Genre
                                           {
                                               Id = g.Id,
                                               GenreName = g.GenreName
                                           };

            //Main Query
            var movies = (IQueryable<Movie>)_context.Movie
                .Include(m => m.Genre);

            if (gridOptions.FilterList.ContainsKey("title") && !string.IsNullOrWhiteSpace(gridOptions.FilterList["title"]))
                movies = movies.Where(m => m.Title.Contains(title));
            int genreId;
            if (gridOptions.FilterList.ContainsKey("GenreId") && int.TryParse(gridOptions.FilterList["GenreId"], out genreId))
                movies = movies.Where(m => (m.GenreId == genreId));
            DateTime dl;
            if (gridOptions.FilterList.ContainsKey("ReleaseDate_low") && DateTime.TryParse(gridOptions.FilterList["ReleaseDate_low"], out dl))
                movies = movies.Where(m =>  (m.ReleaseDate >= dl));
            DateTime dh;
            if (gridOptions.FilterList.ContainsKey("ReleaseDate_high") && DateTime.TryParse(gridOptions.FilterList["ReleaseDate_high"], out dh))
                movies = movies.Where(m => (m.ReleaseDate <= dh));
            int p;
            if (gridOptions.FilterList.ContainsKey("Price_Low") && int.TryParse(gridOptions.FilterList["Price_Low"], out p))
                movies = movies.Where(m => (m.Price >= p));     
            if (gridOptions.FilterList.ContainsKey("Price_High") && int.TryParse(gridOptions.FilterList["Price_High"], out p))
                movies = movies.Where(m => (m.Price <= p));
                   
            

            // Collect total row count information for pagination
            gridOptions.PaginationOptions.Count = movies.Count();
            var page = (!currentPage.HasValue || currentPage.Value == 0) ? 1 : currentPage.Value;
            gridOptions.PaginationOptions.CurrentPage = page;


            //Order the data if a sort option is selected
            if (!String.IsNullOrWhiteSpace(gridOptions.SelectedSortName))
                movies = movies.OrderBy<Movie>(gridOptions.SelectedSortName
                    , gridOptions.SelectedSortDirection == SortableHeaderTagHelper.SortDirection.Desc);

            List<Movie> movieList;
            
            // Perform pagination if the page size is set to a positive number
            if (gridOptions.PaginationOptions.PageSize > 0)
            {
                movieList = await movies
                    .Skip((page - 1) * gridOptions.PaginationOptions.PageSize)
                    .Take(gridOptions.PaginationOptions.PageSize)
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

            return View(movieGenreVM);        }
```