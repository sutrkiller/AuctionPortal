using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BL.Utils.Enums;
using DAL.Enums;

namespace BL.DTOs.Filters
{
    public class AuctionFilter
    { 
        public long SellerId { get; set; }
        public long[] CategoryIds { get; set; }
        [DisplayName("Ends After"),DataType(DataType.DateTime)]
        public DateTime? StartDate { get; set; }
        [DisplayName("Ends Before"), DataType(DataType.DateTime)]
        public DateTime? EndDate { get; set; }
        public DeliveryType[] DeliveryOptions { get; set; }
        public PaymentMethod[] PaymentOptions { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public bool OnlyCurrentAuctions { get; set; }

        public AuctionSortCriteria SortCriteria { get; set; }
        public bool SortAscending { get; set; } = true;
        public long HighestBidderId { get; set; }
        public long BidderId { get; set; }
    }
}
