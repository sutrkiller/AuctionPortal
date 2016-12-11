using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DAL.Enums;
using DelegateDecompiler;
using Riganti.Utils.Infrastructure.Core;

namespace DAL.Entities
{
    public class Auction : IEntity<long>
    {
        public long ID { get; set; }
        //public long SellerId { get; set; }
        [Required]
        public DateTime AuctionStart { get; set; }
        [Required]
        public DateTime AuctionEnd { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal BasePrice { get; set; }
        public long AuctionViews { get; set; }
        public string DeliveryOptionsSerialized { get; set; } //Format: 1;2;3
        public string PaymentOptionsSerialized { get; set; } //Format: 1;2;3

        public virtual Delivery Delivery { get; set; }
        public virtual User Seller { get; set; }
        public virtual ICollection<Item> Items { get; set; } = new HashSet<Item>();
        public virtual ICollection<Bid> Bids { get; set; } = new HashSet<Bid>();
        public virtual Category Category { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        //[NotMapped]
        //[Computed]
        //public DeliveryType[] DeliveryOptions
        //{
        //    get { return DeliveryOptionsSerialized?.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(x => (DeliveryType)Convert.ToInt32(x)).ToArray() ?? new DeliveryType[0]; }
        //    set
        //    {
        //        DeliveryOptionsSerialized = value == null ? "" : string.Join(";", value.Cast<int>());
        //    }
        //}

        //[NotMapped]
        //[Computed]
        //public PaymentMethod[] PaymentOptions
        //{
        //    get { return PaymentOptionsSerialized?.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(x => (PaymentMethod)Convert.ToInt32(x)).ToArray() ?? new PaymentMethod[0]; }
        //    set
        //    {
        //        PaymentOptionsSerialized = value == null ? "" : string.Join(";", value.Cast<int>());
        //    }
        //}

        //[NotMapped]
        //[Computed]
        //public bool Ended => AuctionEnd < DateTime.Now; //-> BL

        //[NotMapped]
        //[Computed]
        //public decimal CurrentPrice => (Bids?.OrderByDescending(b => b.BidTime).FirstOrDefault()?.Value ?? BasePrice); //-> BL
    }
}
