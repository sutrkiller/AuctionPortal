using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BL.DTOs.Deliveries;
using BL.DTOs.Users;

namespace PL.Models
{
    public class DeliveryDetailsViewModel
    {
        public DeliveryDTO Delivery { get; set; }
        public UserDTO Seller { get; set; }
    }
}