using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Service
{
    public class PagedResult
    {
        public PagedResult()
        {

        }

        public PagedResult(int pageNumber, int pageSize, long total)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalItems = total;
            TotalPages = (int)Math.Ceiling((double)total / PageSize);
        }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 2;
        public long TotalItems { get; set; } = 0;
        public int TotalPages { get; set; } = 0;
    }
}
