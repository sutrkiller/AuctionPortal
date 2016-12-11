using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Bids
{
    [Serializable]
    public class BidDTO : BaseDTO
    {
        public long ID { get; set; }
        public long AuctionId { get; set; }
        public long BidderId { get; set; }
        public DateTime BidTime { get; set; }
        public decimal Value { get; set; }
    }
}
