using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using BL.DTOs.Auctions;
using BL.DTOs.Bids;
using BL.DTOs.Categories;
using BL.DTOs.Filters;
using BL.DTOs.ItemImages;
using BL.DTOs.Items;
using BL.Facades;
using BL.Utils.Claims;
using BL.Utils.Enums;
using DAL.Enums;
using PL.Models;
using X.PagedList;

namespace PL.Controllers
{
    public class AuctionsController : Controller
    {
        private readonly string _filterSesssionKey = "Filter";
        private readonly string _categoryTreesSessionKey = "Categories";
        public AuctionFacade AuctionFacade { get; set; }
        public UserFacade UserFacade { get; set; }

        /// <summary>
        /// GET: ~/Auctions -main view of auctions
        /// </summary>
        /// <param name="page">Number of to display</param>
        /// <returns>view</returns>
        public ActionResult Index(int page = 1)
        {
            var filter = Session[_filterSesssionKey] as AuctionFilter ?? new AuctionFilter {SortCriteria = AuctionSortCriteria.AuctionEnd};
            var categories = Session[_categoryTreesSessionKey] as IList<CategoryDTO>;

            var result = AuctionFacade.GetAuctions(filter, page);
           // result.ResultPage = result.ResultPage.Where(x => !x.Ended).Concat(result.ResultPage.Where(x => x.Ended));

            var model = InitializeAuctionListViewModel(result, categories);

            return View("AuctionList", model);
        }

        /// <summary>
        /// When filter applied
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(AuctionListViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = InitializeAuctionListViewModel(AuctionFacade.GetAuctions(null));
                return View("AuctionList", model);
            }

            model.Filter.CategoryIds = ProcessCategoryIds(model);

            Session[_filterSesssionKey] = model.Filter;
            Session[_categoryTreesSessionKey] = model.Categories;

            var result = AuctionFacade.GetAuctions(model.Filter);
            //result.ResultPage = result.ResultPage.Where(x => !x.Ended).Concat(result.ResultPage.Where(x => x.Ended));

            var newModel = InitializeAuctionListViewModel(result, model.Categories);

            return View("AuctionList", newModel);
        }

        /// <summary>
        /// Return filter to defaults.
        /// </summary>
        /// <returns></returns>
        public ActionResult ClearFilter()
        {
            Session[_filterSesssionKey] = null;
            Session[_categoryTreesSessionKey] = null;
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Display details of single auction
        /// </summary>
        /// <param name="id">Id of auction to display</param>
        /// <returns></returns>
        public ActionResult Details(long id)
        {
            var model = AuctionFacade.GetAuction(id);
            var category = AuctionFacade.GetCategory(model.CategoryId);
            var items = AuctionFacade.GetItemsForAuction(model.ID);
            var itemViews =
                items.Select(i => new ItemViewModel() {Item = i, Images = AuctionFacade.GetImagesForItem(i.ID).ToList()})
                    .ToList();
            var comments =
                AuctionFacade.GetComments(new CommentFilter() {AuctionId = model.ID})
                    .ResultPage.OrderBy(x => x.Time)
                    .ToList();
            var detail = new AuctionDetailViewModel()
            {
                Auction = model,
                Category = category,
                Items = itemViews,
                Bid = model.MinPrice,
                Comments = comments
            };

            return View("AuctionDetails", detail);
        }

        /// <summary>
        /// When user enters custom bid.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BidCustom(AuctionDetailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Details(model.Auction.ID);
            }
            bool success;
            CreateBid(model.Auction.ID, model.Bid, out success);
            if (!success)
            {
                ModelState.AddModelError("model.Bid", (string) TempData["ErrorMessage"]);
                return Details(model.Auction.ID);
            }
            return RedirectToAction("ListBought","AuctionsManagment");
        }

        /// <summary>
        /// When user bids default minimum.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bid"></param>
        /// <returns></returns>
        [Authorize(Roles = Claims.Authenticated)]
        public ActionResult Bid(long id, decimal bid)
        {
            bool success;
            return CreateBid(id, bid,out success);
        }

        private ActionResult CreateBid(long auctionId, decimal bid, out bool success )
        {
            success = false;
            var user = UserFacade.GetUser(User.Identity);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found";
                return RedirectToAction("ListBought", "AuctionsManagment");
            }
            var userId = user.ID;
            var bidDto = new BidDTO() { AuctionId = auctionId, BidderId = userId, Value = bid };

            var auction = AuctionFacade.GetAuction(auctionId);
            if (auction.SellerId == userId)
            {
                TempData["ErrorMessage"] = "You cannot bid on your own auction.";
            }
            else
            {
                try
                {
                    success = AuctionFacade.BidOnAuction(bidDto);
                    if (!success)
                    {
                        TempData["ErrorMessage"] = "Value must be higher then current highest bid.";
                    }
                    TempData["MessageTitle"] = "Bid successful";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessageTitle"] = "Bid unsuccessful: ";
                    TempData["ErrorMessage"] = ex.Message;
                }
            }
            return RedirectToAction("ListBought","AuctionsManagment");
        }

        /// <summary>
        /// When user buys item immediately with OneClickBuy price.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bid"></param>
        /// <returns></returns>
        [Authorize(Roles = Claims.Authenticated)]
        public ActionResult Buy(long id, decimal bid)
        {
            bool success;
            var result = CreateBid(id, bid,out success);
            if (success)
            {
               // AuctionFacade.OnAuctionEnd(id);
            }
            return result;
        }

        /// <summary>
        /// When user wants to create auction. Base GET to display create form.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = Claims.Authenticated)]
        public ActionResult CreateAuction()
        {
            var model = new AuctionCreateViewModel();
            model = InitializeCreateViewModel(model);

            model.AuctionEnd = DateTime.Now.AddDays(7);

            return View("CreateAuction", model);
        }


        private const string TmpFilesPath = @"~\Content\Images\Tmp";

        /// <summary>
        /// When user submits some part of form.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="command">When command == 'Create' => creates auction from model. When command == 'Add item' => add new item together with its loaded images to model.auction.items. When command == [long] => delete item from model with ID == command.</param>
        /// <returns></returns>
        [Authorize(Roles = Claims.Authenticated)]
        [HttpPost]
        public ActionResult CreateAuction(AuctionCreateViewModel model, string command)
        {
            if (command == "Create")
            {
                return AddAuction(model);
            }
            else if (command == "Add item")
            {
                return AddItemToAuction(model);
            }
            else
            {
                return DeleteItemImage(model, command);
            }
        }

        private ActionResult DeleteItemImage(AuctionCreateViewModel model, string command)
        {
            InitializeCreateViewModel(model);
            var i = Convert.ToInt32(command);
            //Request.Form.Remove("removed_index");
            model.Items.RemoveAt(i);
            ModelState.Clear();
            return View("CreateAuction", model);
        }

        private ActionResult AddAuction(AuctionCreateViewModel model)
        {
            var email = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null)
            {
                TempData["ErrorMessage"] = "User not recognized.";
                return View("CreateAuction", InitializeCreateViewModel(model));
            }
            long? userId = null;
            try
            {
                userId = UserFacade.GetUserByEmail(email)?.ID;
            }
            catch (Exception)
            {
                userId = null;
            }

            if (userId == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return View("CreateAuction", InitializeCreateViewModel(model));
            }

            ModelState.Remove(nameof(AuctionCreateViewModel.ItemName));
            if (!ModelState.IsValid || model.BasePrice <= 0)
            {
                if (model.CategoryId == 0)
                {
                    ModelState.AddModelError("Categories", @"Select valid category please.");
                }
                if (model.BasePrice <= 0)
                {
                    ModelState.AddModelError(nameof(model.BasePrice), @"Price must be a positive value.");
                }
                return View("CreateAuction", InitializeCreateViewModel(model));
            }
            var dto = new AuctionCreateDTO()
            {
                AuctionEnd = model.AuctionEnd,
                AuctionStart = DateTime.Now,
                BasePrice = model.BasePrice,
                SellerId = userId.Value,
                DeliveryOptions =
                    model.DeliveryTypes.Any() ? model.DeliveryTypes.ToArray() : new[] { DeliveryType.Unknown },
                PaymentOptions =
                    model.PaymentMethods.Any() ? model.PaymentMethods.ToArray() : new[] { PaymentMethod.Unknown },
                CategoryId = model.CategoryId
            };
            var items =
                model.Items.Select(
                        x =>
                            new ItemCreateDTO()
                            {
                                Item = new ItemDTO()
                                {
                                    Name = x.Name,
                                    Description = x.Description
                                },
                                Images = x.Images?.Select(image => new ItemImageDTO()
                                {
                                    ImagePath = image
                                }).ToList() ?? new List<ItemImageDTO>()
                            })
                    .ToList();
            AuctionFacade.CreateAuction(dto, items);
            return RedirectToAction("Index", "Auctions");
        }

        private ActionResult AddItemToAuction(AuctionCreateViewModel model)
        {
            InitializeCreateViewModel(model);
            if (!ModelState.IsValidField(nameof(AuctionCreateViewModel.ItemName)))
            {
                ModelState.Clear();
                ModelState.AddModelError(nameof(AuctionCreateViewModel.ItemName), "Field Name is required.");
                return View("CreateAuction", model);
            }

            var cvm = new ItemCreateViewModel()
            {
                Name = model.ItemName,
                Description = model.ItemDescription,
                Images = new List<string>()
            };
            for (int i = 0; i < Request.Files.Count; ++i)
            {

                var postedfile = Request.Files[i];
                if (postedfile != null && postedfile.ContentLength > 0)
                {
                    try
                    {
                        var file = new WebImage(postedfile.InputStream) { FileName = postedfile.FileName };
                        var newName = Path.Combine(TmpFilesPath, Guid.NewGuid() + "_" + file.FileName);
                        file.Save(newName);
                        cvm.Images.Add(newName);
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("model.Images", "Some files were unable to save.");
                        return View("CreateAuction", model);
                    }
                }
            }
            model.Items.Add(cvm);


            model.ItemName = "";
            model.ItemDescription = "";
            ModelState.Clear();
            return View("CreateAuction", model);
        }

        #region Private Methods

        private AuctionListViewModel InitializeAuctionListViewModel(AuctionListQueryResultDTO result,
            IList<CategoryDTO> categories = null)
        {
            return new AuctionListViewModel
            {
                Auctions = new StaticPagedList<AuctionDTO>(result.ResultPage, result.RequestedPage, AuctionFacade.AuctionsPageSize, result.TotalResultCount),
                Categories = categories ?? AuctionFacade.GetAllCategories() as IList<CategoryDTO>,
                Filter = result.Filter
            };
        }

        private static long[] ProcessCategoryIds(AuctionListViewModel model)
        {
            return model.Categories.Where(x => x.IsActive).Select(x => x.ID).ToArray();
        }

        private AuctionCreateViewModel InitializeCreateViewModel(AuctionCreateViewModel model)
        {

            var categories = AuctionFacade.GetAllCategories();
            var categoriesList = categories.Select(d => new { Id = d.ID, Value = d.Name });
            model.Categories = new SelectList(categoriesList, "Id", "Value");

            var deliveryTypes = Enum.GetNames(typeof(DeliveryType));
            var deliveriesList =
                deliveryTypes.Select(x => new { Id = (DeliveryType)Enum.Parse(typeof(DeliveryType), x), Value = x });
            model.DeliveryTypesList = new SelectList(deliveriesList, "Id", "Value");

            var paymentTypes = Enum.GetNames(typeof(PaymentMethod));
            var paymentsList =
                paymentTypes.Select(x => new { Id = (PaymentMethod)Enum.Parse(typeof(PaymentMethod), x), Value = x });
            model.PaymentMethodsList = new SelectList(paymentsList, "Id", "Value");
            return model;
        }

        #endregion

    }
}