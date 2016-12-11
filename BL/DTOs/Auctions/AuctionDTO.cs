using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Enums;
using DelegateDecompiler;

namespace BL.DTOs.Auctions
{
    [Serializable]
    public class AuctionDTO : BaseDTO
    {
        public long ID { get; set; }
        public long SellerId { get; set; }
        public long CategoryId { get; set; }
        public DateTime AuctionStart { get; set; }
        public DateTime AuctionEnd { get; set; }
        public decimal BasePrice { get; set; }
        public long AuctionViews { get; set; }
        public string DeliveryOptionsSerialized { get; set; } //Format: 1;2;3
        public string PaymentOptionsSerialized { get; set; } //Format: 1;2;3

        public string CoverImagePath { get; set; }
        public decimal MinPrice { get; set; }
        public decimal OneClickPrice { get; set; }

        [NotMapped]
        [Computed]
        public DeliveryType[] DeliveryOptions
        {
            get { return DeliveryOptionsSerialized?.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(x => (DeliveryType)Convert.ToInt32(x)).ToArray() ?? new DeliveryType[0]; }
            set
            {
                DeliveryOptionsSerialized = value == null ? "" : string.Join(";", value.Cast<int>());
            }
        }

        [NotMapped]
        [Computed]
        public PaymentMethod[] PaymentOptions
        {
            get { return PaymentOptionsSerialized?.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(x => (PaymentMethod)Convert.ToInt32(x)).ToArray() ?? new PaymentMethod[0]; }
            set
            {
                PaymentOptionsSerialized = value == null ? "" : string.Join(";", value.Cast<int>());
            }
        }

        [NotMapped]
        [Computed]
        public bool Ended => AuctionEnd < DateTime.Now || CurrentPrice >= OneClickPrice; //-> BL

        public decimal CurrentPrice { get; set; }
    }
}
