using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Enums;

namespace BL.DTOs.Deliveries
{
    [Serializable]
    public class DeliveryDTO : BaseDTO
    {
        public long ID { get; set; }
        public long SellerId { get; set; }
        public long BuyerId { get; set; }
        public long AuctionId { get; set; }
        //public long DeliveryAddressId { get; set; }
        [DisplayName("Delivery address")]
        public string DeliveryAddress { get; set; }
        [DisplayName("Delivery type")]
        public DeliveryType DeliveryType { get; set; }
        [DisplayName("Payment method")]
        public PaymentMethod PaymentMethod { get; set; }
        [DisplayName("Delivery status")]
        public DeliveryStatus DeliveryStatus { get; set; }
    }
}
