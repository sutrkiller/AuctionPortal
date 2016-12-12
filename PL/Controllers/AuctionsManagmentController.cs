using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using BL.DTOs.Auctions;
using BL.DTOs.Deliveries;
using BL.DTOs.Filters;
using BL.Facades;
using BL.Utils.Claims;
using BL.Utils.Enums;
using Castle.MicroKernel.ModelBuilder.Descriptors;
using DAL.Enums;
using PL.Models;
using X.PagedList;

namespace PL.Controllers
{
    [Authorize(Roles = Claims.Admin + ", " + Claims.Authenticated)]
    public class AuctionsManagmentController : Controller
    {
        public AuctionFacade AuctionFacade { get; set; }
        public UserFacade UserFacade { get; set; }

        public ActionResult ListCreated(int page = 1)
        {
            var user = UserFacade.GetUser(User.Identity);
            if (user == null)
            {
                TempData["ErrorMessageTitle"] = "User not found";
                return RedirectToAction("Index", "Auctions");
            }

            var auctions =
                AuctionFacade.GetAuctions(
                    new AuctionFilter
                    {
                        SellerId = user.ID,
                        SortAscending = true,
                        SortCriteria = AuctionSortCriteria.AuctionEnd
                    }, page);

            var model = PrepareModel(auctions, user.ID);
            return View("Created", model);
        }

        private MyAuctionsViewModel PrepareModel(AuctionListQueryResultDTO auctions, long userId)
        {
            var model = new MyAuctionsViewModel();
            model.Auctions = new StaticPagedList<AuctionDTO>(auctions.ResultPage, auctions.RequestedPage,
                AuctionFacade.AuctionsPageSize, auctions.TotalResultCount);

            foreach (var auction in model.Auctions)
            {
                model.UserBids.Add(auction.ID,
                    AuctionFacade.GetBids(new BidFilter()
                    {
                        AuctionId = auction.ID,
                        SortAscending = false,
                        BidderId = userId
                    }).ResultPage?.FirstOrDefault());
            }

            foreach (var auction in model.Auctions)
            {
                model.Deliveries.Add(auction.ID, AuctionFacade.GetAuctionDelivery(auction.ID));
            }
            return model;
        }

        public ActionResult ListBought(int page = 1)
        {
            var user = UserFacade.GetUser(User.Identity);
            if (user == null)
            {
                TempData["ErrorMessageTitle"] = "User not found";
                return RedirectToAction("Index", "Auctions");
            }

            var auctions =
                AuctionFacade.GetAuctions(
                    new AuctionFilter
                    {
                        BidderId = user.ID,
                        SortCriteria = AuctionSortCriteria.AuctionEnd,
                        SortAscending = true
                    }, page);
            return View("Bidded",
                PrepareModel(auctions, user.ID));
        }

        public ActionResult RequestDelivery(long id)
        {
            try
            {
                var auction = AuctionFacade.GetAuction(id);
                if (auction == null) throw new NullReferenceException("Referenced auction not found.");
                var user = UserFacade.GetUser(User.Identity);
                if (user == null) throw new NullReferenceException("User not found.");
                var model = new DeliveryCreateViewModel
                {
                    AuctionId = auction.ID,
                    BuyerId = user.ID,
                    SellerId = auction.SellerId,
                    DeliveryAddress = user.Address,
                    DeliveryTypesList = PrepareDeliveryList(auction.DeliveryOptions),
                    PaymentMethodsList = PreparePaymentList(auction.PaymentOptions)
                };
                return View("CreateDelivery", model);

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("ListBought");
            }
        }

        private SelectList PreparePaymentList(PaymentMethod[] paymentMethods)
        {
            if (paymentMethods.Contains(PaymentMethod.Unknown))
                return
                    new SelectList(
                        Enum.GetNames(typeof(PaymentMethod))
                            .Select((x, i) => new {Id = (PaymentMethod) Enum.Parse(typeof(PaymentMethod), x), Value = x})
                            .Where(x => x.Id != PaymentMethod.Unknown), "Id", "Value");
            return new SelectList(paymentMethods.Select(x => new {Id = x, Value = x.ToString()}), "Id", "Value");
        }

        private SelectList PrepareDeliveryList(DeliveryType[] deliveryTypes)
        {
            if (deliveryTypes.Contains(DeliveryType.Unknown))
                return
                    new SelectList(
                        Enum.GetNames(typeof(DeliveryType))
                            .Select((x, i) => new {Id = (DeliveryType) Enum.Parse(typeof(DeliveryType), x), Value = x})
                            .Where(x => x.Id != DeliveryType.Unknown), "Id", "Value");
            return new SelectList(deliveryTypes.Select(x => new {Id = x, Value = x.ToString()}), "Id", "Value");
        }

        private SelectList PrepareDeliveryStatusList(DeliveryStatus current)
        {
            return new SelectList(Enum.GetNames(typeof(DeliveryStatus))
                .Select((x, i) => new {Id = (DeliveryStatus) Enum.Parse(typeof(DeliveryStatus), x), Value = x})
                .Where(x => x.Id >= current), "Id", "Value");
        }

        public ActionResult DeliveryDetails(long id)
        {
            var delivery = AuctionFacade.GetDelivery(id);
            if (delivery == null) return RedirectToAction("ListBought");
            var seller = UserFacade.GetUser(delivery.SellerId);
            var model = new DeliveryDetailsViewModel
            {
                Delivery = delivery,
                Seller = seller
            };

            return View("DeliveryDetails", model);
        }

        public ActionResult CreateDelivery(DeliveryCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var auction = AuctionFacade.GetAuction(model.AuctionId);
                model.DeliveryTypesList = PrepareDeliveryList(auction.DeliveryOptions);
                model.PaymentMethodsList = PreparePaymentList(auction.PaymentOptions);
                return View("CreateDelivery", model);
            }

            try
            {
                var deliveryDto = new DeliveryDTO
                {
                    AuctionId = model.AuctionId,
                    BuyerId = model.BuyerId,
                    DeliveryAddress = model.DeliveryAddress,
                    DeliveryStatus = DeliveryStatus.Processing,
                    DeliveryType = model.DeliveryType,
                    PaymentMethod = model.PaymentMethod,
                    SellerId = model.SellerId
                };
                AuctionFacade.CreateDelivery(deliveryDto);
                TempData["Message"] = "Delivery requested. Proccessing by seller.";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Request failed.";
                var auction = AuctionFacade.GetAuction(model.AuctionId);
                model.DeliveryTypesList = PrepareDeliveryList(auction.DeliveryOptions);
                model.PaymentMethodsList = PreparePaymentList(auction.PaymentOptions);
                return View("CreateDelivery", model);
            }

            return RedirectToAction("ListBought");

        }

        public ActionResult EditDelivery(long id)
        {
            try
            {
                var delivery = AuctionFacade.GetDelivery(id);
                if (delivery == null) throw new NullReferenceException("Delivery not found.");
                var user = UserFacade.GetUser(delivery.BuyerId);
                if (user == null) throw new NullReferenceException("Buyer not found.");
                var model = new DeliveryEditViewModel()
                {
                    Id = delivery.ID,
                    BuyerId = delivery.BuyerId,
                    BuyerName = user.FirstName + " " + user.LastName,
                    BuyerEmail = user.Email,
                    DeliveryType = delivery.DeliveryType.ToString(),
                    PaymentMethod = delivery.PaymentMethod.ToString(),
                    DeliveryAddress = delivery.DeliveryAddress,
                    DeliveryStatus = delivery.DeliveryStatus,
                    DeliveryStatuses = PrepareDeliveryStatusList(delivery.DeliveryStatus)
                };
                return View("EditDelivery", model);

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("ListCreated");
            }
        }

        [HttpPost]
        public ActionResult EditDelivery(DeliveryEditViewModel model)
        {
            try
            {
                if (!ModelState.IsValid) throw new FormatException();
                var delivery = AuctionFacade.GetDelivery(model.Id);
                if (delivery == null) throw new NullReferenceException("Delivery not found.");
                var editDto = new DeliveryEditDTO
                {
                    DeliveryAddress = delivery.DeliveryAddress,
                    ID = delivery.ID,
                    DeliveryType = delivery.DeliveryType,
                    PaymentMethod = delivery.PaymentMethod,
                    DeliveryStatus = model.DeliveryStatus
                };

                AuctionFacade.EditDelivery(editDto);
                TempData["Message"] = "Delivery status changes";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Editing failed";
            }
            return RedirectToAction("ListCreated");
        }
    }
}