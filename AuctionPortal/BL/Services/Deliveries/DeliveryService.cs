using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BL.DTOs;
using BL.DTOs.Auctions;
using BL.DTOs.Deliveries;
using BL.DTOs.Users;
using BL.Repositories;
using DAL.Entities;
using DAL.Enums;

namespace BL.Services.Deliveries
{
    public class DeliveryService : BaseService, IDeliveryService
    {
        #region Dependencies

        private readonly DeliveryRepository _deliveryRepository;
        private readonly UserRepository _userRepository;
        private readonly AuctionRepository _auctionRepository;

        public DeliveryService(DeliveryRepository deliveryRepository, UserRepository userRepository, AuctionRepository auctionRepository)
        {
            _deliveryRepository = deliveryRepository;
            _userRepository = userRepository;
            _auctionRepository = auctionRepository;
        }

        #endregion

        public void CreateDelivery(DeliveryDTO deliveryDTO)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                //var delivery = Mapper.Map<Delivery>(deliveryDTO);
                var delivery = new Delivery();
                delivery.Auction = GetAuctionById(deliveryDTO.AuctionId);
                delivery.Seller = GetUserById(deliveryDTO.SellerId);
                delivery.Buyer = GetUserById(deliveryDTO.BuyerId);
                delivery.DeliveryStatus = DeliveryStatus.Processing;
                //TODO: change this later
                delivery.DeliveryAddress = "Unknown";
                delivery.DeliveryType = DeliveryType.Unknown;
                delivery.PaymentMethod = PaymentMethod.Unknown;
                _deliveryRepository.Insert(delivery);
                uow.Commit();
            }
        }

        public void EditDelivery(DeliveryEditDTO deliveryDTO)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var del = _deliveryRepository.GetById(deliveryDTO.ID);
                Mapper.Map(deliveryDTO, del);

                _deliveryRepository.Update(del);
                uow.Commit();
            }
        }

        public void DeleteDelivery(long deliveryId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                _deliveryRepository.Delete(deliveryId);
                uow.Commit();
            }
        }

        public DeliveryDTO GetDelivery(long deliveryId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var del = _deliveryRepository.GetById(deliveryId);
                return del != null ? Mapper.Map<DeliveryDTO>(del) : null;
            }
        }

        public DeliveryDTO GetAuctionDelivery(long auctionId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var auc = _auctionRepository.GetById(auctionId);
                return auc != null ? Mapper.Map<DeliveryDTO>(auc.Delivery) : null;
            }
        }

        private User GetUserById(long userId)
        {
            var usr = _userRepository.GetById(userId);
            if (usr == null)
            {
                throw new NullReferenceException($"{GetType().Name} - {nameof(GetUserById)} buyer/seller cannot be null.");
            }
            return usr;
        }

        private Auction GetAuctionById(long auctionId)
        {
            var auc = _auctionRepository.GetById(auctionId);
            if (auc == null)
            {
                throw new NullReferenceException($"{GetType().Name} - {nameof(GetAuctionById)} auction cannot be null.");
            }
            return auc;
        }
    }
}
