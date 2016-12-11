using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Common
{
    public abstract class PagedListQueryResultDTO<T>
    {
        public int TotalResultCount { get; set; }

        public int RequestedPage { get; set; }

        public IEnumerable<T> ResultPage { get; set; }
    }
}
