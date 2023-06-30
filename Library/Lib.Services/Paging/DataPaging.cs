using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.Services.Paging
{
    public class PagedResults<T> where T : class
    {
        public int TotalRecords { get; set; }
        public int CurrentPageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
        public T Items { get; set; }
        public PagedResults(T data, int totalRecords, int currentPageNumber, int pageSize)
        {
            Items = data;
            TotalRecords = totalRecords;
            CurrentPageNumber = currentPageNumber;
            PageSize = pageSize;

            // total pages count
            TotalPages = Convert.ToInt32(Math.Ceiling(TotalRecords / (double)pageSize));

            HasNextPage = CurrentPageNumber < TotalPages;
            HasPreviousPage = CurrentPageNumber > 1;
        }
    }
}
