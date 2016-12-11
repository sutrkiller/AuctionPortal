using System;
using System.Collections.Generic;
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
        public string DeliveryAddress { get; set; }
        public DeliveryType DeliveryType { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
    }
}
