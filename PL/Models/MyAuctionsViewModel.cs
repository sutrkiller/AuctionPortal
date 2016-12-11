using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BL.DTOs.Auctions;
using BL.DTOs.Bids;
using BL.DTOs.Deliveries;
using X.PagedList;

namespace PL.Models
{
    public class MyAuctionsViewModel
    {
        public StaticPagedList<AuctionDTO> Auctions { get; set; }
        public Dictionary<long, DeliveryDTO> Deliveries { get; set; } = new Dictionary<long, DeliveryDTO>();
        public Dictionary<long, BidDTO> UserBids { get; set; } = new Dictionary<long, BidDTO>();
    }
}