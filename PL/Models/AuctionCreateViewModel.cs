using DAL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PL.Models
{
    public class AuctionCreateViewModel
    {
        public long SellerId { get; set; }
        [Required, DisplayName("Category")]
        public long CategoryId { get; set; }
        [DisplayName("Category")]
        public SelectList Categories { get; set; }
        [Required, DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm:ss}",ApplyFormatInEditMode = true), DisplayName("Auction end")]
        public DateTime AuctionEnd { get; set; }
        [Required, DataType(DataType.Currency), DisplayName("Base price")]
        public decimal BasePrice { get; set; }
        [DisplayName("Supported delivery types")]
        public ICollection<DeliveryType> DeliveryTypes { get; set; } = new List<DeliveryType>();
        public SelectList DeliveryTypesList { get; set; }
        [DisplayName("Supported payment methods")]
        public ICollection<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();
        public SelectList PaymentMethodsList { get; set; }
        public IList<ItemCreateViewModel> Items { get; set; } = new List<ItemCreateViewModel>();

        [Required,DisplayName("Name")]
        public string ItemName { get; set; }
        [DisplayName("Description")]
        public string ItemDescription { get; set; }
    }
}