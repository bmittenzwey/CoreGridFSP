# PaginationOptions

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

