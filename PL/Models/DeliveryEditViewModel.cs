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
    public class DeliveryEditViewModel
    {
        public long Id { get; set; }
        public long BuyerId { get; set; }
        [DisplayName("Buyer")]
        public string BuyerName { get; set; }
        [DisplayName("Delivery address")]
        public string DeliveryAddress { get; set; }
        [DisplayName("Delivery type")]
        public string DeliveryType { get; set; }
        [DisplayName("Payment method")]
        public string PaymentMethod { get; set; }
        [Required,DisplayName("Delivery status")]
        public DeliveryStatus DeliveryStatus { get; set; }
        [DisplayName("Delivery status")]
        public SelectList DeliveryStatuses { get; set; }
    }
}