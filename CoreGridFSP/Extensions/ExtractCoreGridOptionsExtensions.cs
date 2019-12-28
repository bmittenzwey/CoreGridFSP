using CoreGridFSP.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreGridFSP.Extensions
{
    public static class ExtractCoreGridOptionsExtensions
    {
        /// <summary>
        /// Extracts core grid options from a request query string 
        /// </summary>
        /// <param name="request">The HttpRequest</param>
        /// <returns>A populated CoreGridFSPOptions object</returns>
        public static CoreGridFSPOptions ExtractCoreGridOptions(this HttpRequest request)
        {
            var gridOptions = new CoreGridFSPOptions();
            var paginationOptions = new PaginationOptions() { Count = 1 };

            foreach(var param in request.Query)
            {
                var lkey = param.Key.Trim().ToLower();
                if (lkey == "selectedsort")
                    gridOptions.SelectedSort = param.Value;
                else if (lkey == "currentpage")
                {
                    int page = 1;
                    int.TryParse(param.Value, out page);
                    paginationOptions.CurrentPage = page;
                }
                else if (lkey == "pagesize")
                {
                    int pageSize = 0;
                    if (int.TryParse(param.Value, out pageSize))
                        paginationOptions.PageSize = pageSize;

                }
                else
                {
                    if (gridOptions.FilterList.ContainsKey(param.Key))
                    {
                        gridOptions.FilterList.Add(param.Key, param.Value);
                    }
                    else
                        gridOptions.FilterList[param.Key] = param.Value;
                }
            }

            gridOptions.PaginationOptions = paginationOptions;
            return gridOptions;
        }
    }
}
