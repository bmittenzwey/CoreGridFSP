# Index View Model

The view model wraps the data in a List<>, includes any dropdown select lists, and the CoreGridFSPOptions required for each of the Tag Helpers

```csharp
using CoreGridFSP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MvcMovie.Models
{
    public class MovieGenreViewModel
    {
        public List<Movie> Movies { get; set; }
        public SelectList Genres { get; set; }
        public CoreGridFSPOptions CoreGridOptions { get; set; }
        
    }
}
```