using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Enums;

namespace PL.Models
{
    public class DeliveryCreateViewModel
    {
        public long SellerId { get; set; }
        public long BuyerId { get; set; }
        public long AuctionId { get; set; }
        [Required]
        [DisplayName("Delivery address")]
        public string DeliveryAddress { get; set; }

        [DisplayName("Possible delivery types")]
        [Required]
        public DeliveryType DeliveryType { get; set; } = DeliveryType.Unknown;
        public SelectList DeliveryTypesList { get; set; }

        [DisplayName("Possible payment methods")]
        [Required]
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Unknown;
        public SelectList PaymentMethodsList { get; set; }
    }
}