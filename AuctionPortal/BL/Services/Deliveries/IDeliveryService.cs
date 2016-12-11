using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTOs.Auctions;
using BL.DTOs.Deliveries;
using BL.DTOs.Users;

namespace BL.Services.Deliveries
{
    public interface IDeliveryService
    {
        void CreateDelivery(DeliveryDTO deliveryDTO);
        void EditDelivery(DeliveryEditDTO deliveryDTO);
        void DeleteDelivery(long deliveryId);
        DeliveryDTO GetDelivery(long deliveryId);
        DeliveryDTO GetAuctionDelivery(long auctionId);
    }
}
