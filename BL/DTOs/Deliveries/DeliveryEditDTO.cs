using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Enums;

namespace BL.DTOs.Deliveries
{
    [Serializable]
    public class DeliveryEditDTO : BaseDTO
    {
        public long ID { get; set; }
        public string DeliveryAddress { get; set; }
        public DeliveryType DeliveryType { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
    }
}
