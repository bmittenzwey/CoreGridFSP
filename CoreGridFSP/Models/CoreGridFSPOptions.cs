using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreGridFSP.Models
{
    public class CoreGridFSPOptions
    {
        [Display(Name ="Pagination Options")]
        public PaginationOptions PaginationOptions { get; set; }
        [Display(Name ="Selected Sort")]
        public string SelectedSort 
        {
            get
            {
                if (SelectedSortName == null)
                    return null;
                else
                    return SelectedSortName + SelectedSortDirection;
            }
            set
            {
                if(String.IsNullOrWhiteSpace(value))
                {
                    SelectedSortName = null;
                    SelectedSortDirection = SortableHeaderTagHelper.SortDirection.Asc;
                }
                else
                {
                    value = value.Trim();
                    if(value.ToUpper().EndsWith("ASC"))
                    {
                        SelectedSortDirection = SortableHeaderTagHelper.SortDirection.Asc;
                        SelectedSortName = value.Substring(0, value.Length - 3);
                    }
                    else if(value.ToUpper().EndsWith("DESC"))
                    {
                        SelectedSortDirection = SortableHeaderTagHelper.SortDirection.Desc;
                        SelectedSortName = value.Substring(0, value.Length - 4);

                    }
                    else
                    {
                        SelectedSortDirection = SortableHeaderTagHelper.SortDirection.Asc;
                        SelectedSortName = value;
                    }
                }
            }
        }
        [Display(Name ="Selected Sort Name")]
        public string SelectedSortName { get; set; }
        [Display(Name ="Selected Sort Direction", Description ="")]
        public SortableHeaderTagHelper.SortDirection SelectedSortDirection { get; set; }
        [Display(Name = "Filter List", Description ="KVP list of all filter keys and string converted values.")]
        public Dictionary<string, string> FilterList { get; set; } = new Dictionary<string, string>();
    }
}
