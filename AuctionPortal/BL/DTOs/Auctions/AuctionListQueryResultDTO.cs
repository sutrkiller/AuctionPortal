using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTOs.Common;
using BL.DTOs.Filters;

namespace BL.DTOs.Auctions
{
    public class AuctionListQueryResultDTO : PagedListQueryResultDTO<AuctionDTO>
    {
        public AuctionFilter Filter { get; set; }
    }
}
