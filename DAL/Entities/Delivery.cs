using System.ComponentModel.DataAnnotations;
using DAL.Enums;
using Riganti.Utils.Infrastructure.Core;

namespace DAL.Entities
{
    public class Delivery : IEntity<long>
    {
        public long ID { get; set; }
        //public long SellerId { get; set; }
        //public long BuyerId { get; set; }
        //public long AuctionId { get; set; }
        //public long DeliveryAddressId { get; set; }
        [Required]
        public DeliveryType DeliveryType { get; set; }
        [Required]
        public PaymentMethod PaymentMethod { get; set; }
        [Required]
        public DeliveryStatus DeliveryStatus { get; set; }

        //public Address DeliveryAddress { get; set; }
        [Required]
        public string DeliveryAddress { get; set; }
        public User Seller { get; set; }
        public User Buyer { get; set; }
        public Auction Auction { get; set; }
    }
}
