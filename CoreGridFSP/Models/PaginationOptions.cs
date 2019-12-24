using System;
using System.ComponentModel.DataAnnotations;

namespace CoreGridFSP.Models
{
    public class PaginationOptions
    {
        [Display(Name ="Current Page")]
        public int CurrentPage { get; set; } = 1;
        [Display(Name = "Record Count")]
        public int Count { get; set; }
        [Display(Name ="Page Size")]
        public int PageSize { get; set; } = 4;
        [Display(Name ="Total Pages", Description ="Read-Only count of pages available to view" )]
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        [Display(Name ="Enable Previous", Description = "Read-Only indicator of whether a previous page is available.")]
        public bool EnablePrevious => CurrentPage > 1;
        [Display(Name ="Enable Next", Description ="Read-Only indicator of whether a next page is available.")]
        public bool EnableNext => CurrentPage < TotalPages;
    }
}
