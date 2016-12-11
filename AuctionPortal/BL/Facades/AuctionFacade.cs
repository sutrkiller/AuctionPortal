using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BL.DTOs.Auctions;
using BL.DTOs.Bids;
using BL.DTOs.Categories;
using BL.DTOs.Comments;
using BL.DTOs.Deliveries;
using BL.DTOs.Filters;
using BL.DTOs.ItemImages;
using BL.DTOs.Items;
using BL.Services.Auctions;
using BL.Services.Categories;
using BL.Services.Comments;
using BL.Services.Deliveries;
using BL.Services.Items;
using DAL.Enums;

namespace BL.Facades
{
    public class AuctionFacade
    {
        #region Dependencies

        private readonly IAuctionService _auctionService;
        private readonly ICommentService _commentService;
        private readonly IDeliveryService _deliveryService;
        private readonly IItemService _itemService;
        private readonly ICategoryService _categoryService;

        public AuctionFacade(IAuctionService auctionService, ICommentService commentService, IDeliveryService deliveryService, IItemService itemService, ICategoryService categoryService)
        {
            _auctionService = auctionService;
            _commentService = commentService;
            _deliveryService = deliveryService;
            _itemService = itemService;
            _categoryService = categoryService;
        }

        #endregion

        #region Auctions


        public int AuctionsPageSize => _auctionService.AuctionsPageSize;

        public void CreateAuction(AuctionCreateDTO auctionDTO, List<ItemCreateDTO> items)
        {
            //probably unnecessary and it takes too long
            if (auctionDTO.AuctionStart > auctionDTO.AuctionEnd) throw new FormatException("Auction starts after it ends.");
            if (auctionDTO.BasePrice == 0) throw new FormatException("Auction cannot be free of charge.");
            if (auctionDTO.DeliveryOptions.Length == 0) throw new FormatException("Delivery options cannot be empty");
            if (auctionDTO.PaymentOptions.Length == 0) throw new FormatException("Payment options cannot be empty");
            if (auctionDTO.CategoryId <= 0) throw new FormatException("Category not specified.");
            if (auctionDTO.SellerId <= 0) throw new FormatException("Seller not specified.");

            if (items.Any(x => string.IsNullOrEmpty(x.Item.Name)))
            {
                throw new FormatException("Items have to contain non-empty name.");
            }
            if (items.SelectMany(x => x.Images).Any(image=>string.IsNullOrEmpty(image.ImagePath)))
            {
                throw new FormatException("Some images not found.");
            }

            var auctionId = _auctionService.CreateAuction(auctionDTO);

            foreach (var itemCreate in items)
            { 
                itemCreate.Item.AuctionId = auctionId;
                var itemId = _itemService.CreateItem(itemCreate.Item);

                foreach (var image in itemCreate.Images)
                {
                    image.ItemId = itemId;
                    _itemService.CreateItemImage(image);
                }
            }
        }

        public void DeleteAuction(long auctionId)
        {
            if (auctionId <= 0) throw new NullReferenceException("Auction with this id does not exist.");
            _auctionService.DeleteAuction(auctionId);
        }

        public AuctionDTO GetAuction(long auctionId)
        {
            if (auctionId <= 0) throw new NullReferenceException("Auction with this id does not exist.");
            var auction = _auctionService.GetAuction(auctionId, true);
            auction.OneClickPrice = GetOneClickBuyPrice(auctionId);
            auction.MinPrice = GetMinimumBid(auctionId);
            return auction;
        }

        public decimal GetMinimumBid(long auctionId)
        {
            if (auctionId <= 0) throw new NullReferenceException("Auction with this id does not exist.");
            return _auctionService.GetMinimumBid(auctionId);
        }

        public decimal GetOneClickBuyPrice(long auctionId)
        {
            if (auctionId <= 0) throw new NullReferenceException("Auction with this id does not exist.");
            return _auctionService.GetOneClickBuyPrice(auctionId);
        }

        public AuctionListQueryResultDTO GetAuctions(AuctionFilter filter, int requiredPage = 1)
        {
            var auctions = _auctionService.GetAuctions(filter, requiredPage);
            foreach (var auction in auctions.ResultPage)
            {
                auction.OneClickPrice = GetOneClickBuyPrice(auction.ID);
                auction.MinPrice = GetMinimumBid(auction.ID);
            }
            return auctions;
        }

        #endregion

        #region Bids

        public int BidsPageSize => _auctionService.BidsPageSize;
        public double MinBidRisePercentage => _auctionService.MinBidRisePercentage;
        public double OneClickBuyMultiplier => _auctionService.OneClickBuyMultiplier;

        public bool BidOnAuction(BidDTO bidDTO)
        {
            if (bidDTO.AuctionId <= 0) throw new FormatException($"Auction with id {bidDTO.AuctionId} does not exist.");
            if (bidDTO.BidderId <= 0) throw new FormatException($"User with id {bidDTO.BidderId} does not exist.");
            if (bidDTO.Value < _auctionService.GetMinimumBid(bidDTO.AuctionId)) return false;
            bidDTO.BidTime = DateTime.Now;
            _auctionService.CreateBid(bidDTO);
            return true;
        }

        public BidListQueryResultDTO GetBids(BidFilter filter, int requiredPage = 1)
        {
            return _auctionService.GetBids(filter, requiredPage);
        }

        #endregion

        #region Categories

        public void CreateCategory(CategoryDTO categoryDTO)
        {
            if (string.IsNullOrWhiteSpace(categoryDTO.Name)) throw new FormatException("Name has to be a valid text.");
            _categoryService.CreateCategory(categoryDTO);
        }

        public void EditCategory(CategoryDTO categoryDTO)
        {
            if (string.IsNullOrWhiteSpace(categoryDTO.Name)) throw new FormatException("Name has to be a valid text.");
            if (categoryDTO.ID <= 0) throw new FormatException("Id has to be specified when editing.");
            _categoryService.EditCategory(categoryDTO);
        }

        public void DeleteCategory(long categoryId)
        {
            if (categoryId <= 0) throw new FormatException("Id has to be specified when deleting.");
            _categoryService.DeleteCategory(categoryId);
        }

        public CategoryDTO GetCategory(long categoryId)
        {
            if (categoryId <= 0) throw new FormatException($"Category with id {categoryId} does not exist.");
            return _categoryService.GetCategory(categoryId);
        }

        public long GetCategoryId(string name)
        {
            return _categoryService.GetCategoryId(name);
        }

        public IEnumerable<CategoryDTO> GetAllCategories()
        {
            return _categoryService.GetAllCategories();
        }

        #endregion

        #region Comments

        public int CommentsPageSize => _commentService.CommentsPageSize;

        public void CreateComment(CommentCreateDTO commentDTO)
        {
            if (commentDTO.AuctionId <= 0) throw new FormatException($"Auction with id {commentDTO.AuctionId} does not exist.");
            if (commentDTO.AuthorId <= 0) throw new FormatException($"User with id {commentDTO.AuthorId} does not exist.");
            if (commentDTO.ParentId <= 0) throw new FormatException($"Comment with id {commentDTO.ParentId} does not exist.");
            commentDTO.Time = DateTime.Now;
            _commentService.CreateComment(commentDTO);
        }

        public void EditComment(CommentEditDTO commentDTO)
        {
            if (commentDTO.ID <= 0) throw new FormatException($"Comment with id {commentDTO.ID} does not exist.");
            _commentService.EditComment(commentDTO);
        }

        public void DeleteComment(long commentId)
        {
            if (commentId <= 0) throw new FormatException($"Comment with id {commentId} does not exist.");
            _commentService.DeleteComment(commentId);
        }

        public CommentDTO GetComment(long commentId)
        {
            if (commentId <= 0) throw new FormatException($"Comment with id {commentId} does not exist.");
            return _commentService.GetComment(commentId);
        }

        public CommentListQueryResultDTO GetComments(CommentFilter filter, int requiredPage = 1)
        {
            return _commentService.GetComments(filter, requiredPage);
        }

        #endregion

        #region Deliveries

        /// <summary>
        /// Function to call when auction ends.
        /// </summary>
        /// <param name="auctionId">Id of ended auction</param>
        /// <returns>Id of created delivery, 0 if delivery hasn't been created.</returns>
        public long OnAuctionEnd(long auctionId)
        {
            var auction = _auctionService.GetAuction(auctionId, false);

            var bids = _auctionService.GetBids(new BidFilter() { AuctionId = auctionId, SortAscending = false });

            if (auction.Ended && bids.TotalResultCount == 0) //noone bought it
            {
                return 0;
            }
            _deliveryService.CreateDelivery(new DeliveryDTO() { AuctionId = auctionId, BuyerId = bids.ResultPage.First().BidderId, DeliveryStatus = DeliveryStatus.Processing, SellerId = auction.SellerId });
            return _deliveryService.GetAuctionDelivery(auctionId).ID;
        }

        public void EditDelivery(DeliveryEditDTO deliveryDTO)
        {
            _deliveryService.EditDelivery(deliveryDTO);
        }

        public void DeleteDelivery(long deliveryId)
        {
            _deliveryService.DeleteDelivery(deliveryId);
        }

        public DeliveryDTO GetDelivery(long deliveryId)
        {
            return _deliveryService.GetDelivery(deliveryId);
        }

        public DeliveryDTO GetAuctionDelivery(long auctionId)
        {
            return _deliveryService.GetAuctionDelivery(auctionId);
        }

        #endregion

        #region Items

        public IEnumerable<ItemDTO> GetItemsForAuction(long auctionId)
        {
            if (auctionId <= 0) throw new NullReferenceException("Auction with this id does not exist.");
            return _itemService.GetAllItems(auctionId);
        }

        public IEnumerable<ItemImageDTO> GetImagesForItem(long itemId)
        {
            if (itemId <= 0) throw new NullReferenceException("Item with this id does not exist.");
            return _itemService.GetItemImages(itemId);
        }

        public ItemDTO GetItem(long id)
        {
            if (id <= 0) throw new NullReferenceException("Item with this id does not exist.");
            return _itemService.GetItem(id);
        }

        #endregion
    }
}
