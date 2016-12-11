using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTOs.Common;
using BL.DTOs.Filters;

namespace BL.DTOs.Bids
{
    public class BidListQueryResultDTO : PagedListQueryResultDTO<BidDTO>
    {
        public BidFilter Filter { get; set; }
    }
}
