using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BL.DTOs.Auctions;
using BL.DTOs.Bids;
using BL.DTOs.Filters;
using BL.Queries;
using BL.Repositories;
using BL.Utils.Enums;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Services.Auctions
{
    public class AuctionService : BaseService, IAuctionService
    {

        #region Dependencies

        private readonly AuctionRepository _auctionRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly BidRepository _bidRepository;
        private readonly DeliveryRepository _deliveryRepository;
        private readonly UserRepository _userRepository;
        private readonly AuctionListQuery _auctionListQuery;
        private readonly BidListQuery _bidListQuery;

        public AuctionService(AuctionRepository auctionRepository, BidRepository bidRepository, DeliveryRepository deliveryRepository, CategoryRepository categoryRepository, UserRepository userRepository, AuctionListQuery auctionListQuery, BidListQuery bidListQuery)
        {
            _auctionRepository = auctionRepository;
            _bidRepository = bidRepository;
            _deliveryRepository = deliveryRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _auctionListQuery = auctionListQuery;
            _bidListQuery = bidListQuery;
        }

        #endregion

        public int AuctionsPageSize => 20;
        public int BidsPageSize => 20;
        public double MinBidRisePercentage => 5;
        public double OneClickBuyMultiplier => 100;

        public long CreateAuction(AuctionCreateDTO auctionDTO)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var auction = Mapper.Map<Auction>(auctionDTO);

                auction.Seller = GetUserById(auctionDTO.SellerId);

                auction.Category = GetCategoryById(auctionDTO.CategoryId);

                _auctionRepository.Insert(auction);
                uow.Commit();
                return auction.ID;
            }
        }

        public void DeleteAuction(long auctionId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                _auctionRepository.Delete(auctionId);
                uow.Commit();
            }
        }

        public AuctionDTO GetAuction(long auctionId, bool increaseViews = false)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var auction = _auctionRepository.GetById(auctionId, a => a.Category, a => a.Seller, a => a.Bids, a => a.Comments, a => a.Items, a => a.Items.Select(x => x.ItemImages));
                if (auction == null) return null;
                if (increaseViews)
                {
                    auction.AuctionViews++;
                    _auctionRepository.Update(auction);
                    uow.Commit();
                }
                return Mapper.Map<AuctionDTO>(auction);
            }
        }

        public void CreateBid(BidDTO bidDTO)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var bid = Mapper.Map<Bid>(bidDTO);
                bid.Auction = GetAuctionById(bidDTO.AuctionId);
                bid.User = GetUserById(bidDTO.BidderId);
                _bidRepository.Insert(bid);
                uow.Commit();
            }
        }

        public decimal GetMinimumBid(long auctionId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var auction = GetAuction(auctionId, false);

                var bids = GetBids(new BidFilter() { AuctionId = auctionId }).TotalResultCount;
                //var auction = GetAuctionById(auctionId);
                return bids == 0 ? auction.BasePrice : decimal.Multiply(auction.CurrentPrice, 1 + new decimal(MinBidRisePercentage / 100));
            }
        }

        public decimal GetOneClickBuyPrice(long auctionId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var auction = GetAuctionById(auctionId);
                return decimal.Multiply(auction.BasePrice, new decimal(OneClickBuyMultiplier));
            }
        }

        public AuctionListQueryResultDTO GetAuctions(AuctionFilter filter, int requiredPage = 1)
        {
            using (UnitOfWorkProvider.Create())
            {
                if (filter == null) filter = new AuctionFilter();

                var list = _auctionListQuery;
                list.Filter = filter;
                var result = list.GetFiltered();
                var ordered = result.OrderBy(x => (x.AuctionEnd < DateTime.Now || x.CurrentPrice >= x.BasePrice * new decimal(OneClickBuyMultiplier)) ? 1 : 0);
                switch (filter.SortCriteria)
                {
                    case AuctionSortCriteria.AuctionStart:
                        ordered = filter.SortAscending ? ordered.ThenBy(x => x.AuctionStart) : ordered.ThenByDescending(x => x.AuctionStart);
                        break;
                    case AuctionSortCriteria.AuctionEnd:
                        ordered = filter.SortAscending ? ordered.ThenBy(x => x.AuctionEnd) : ordered.ThenByDescending(x => x.AuctionEnd);
                        break;
                    case AuctionSortCriteria.AuctionViews:
                        ordered = filter.SortAscending ? ordered.ThenBy(x => x.AuctionViews) : ordered.ThenByDescending(x => x.AuctionViews);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                result = ordered.Skip(Math.Max(0, requiredPage - 1) * AuctionsPageSize).Take(AuctionsPageSize);

                return new AuctionListQueryResultDTO()
                {
                    Filter = filter,
                    ResultPage = result.ToList(),
                    RequestedPage = requiredPage,
                    TotalResultCount = ordered.Count()
                };
            }
        }

        public BidListQueryResultDTO GetBids(BidFilter filter, int requiredPage = 1)
        {
            using (UnitOfWorkProvider.Create())
            {
                var query = _bidListQuery;
                query.ClearSortCriterias();
                query.Filter = filter;
                query.Skip = Math.Max(0, requiredPage - 1) * BidsPageSize;
                query.Take = BidsPageSize;

                var sortOrder = filter.SortAscending ? SortDirection.Ascending : SortDirection.Descending;
                query.AddSortCriteria(nameof(Bid.Value), sortOrder);

                return new BidListQueryResultDTO()
                {
                    RequestedPage = requiredPage,
                    TotalResultCount = query.GetTotalRowCount(),
                    ResultPage = query.Execute(),
                    Filter = filter
                };
            }
        }



        private Auction GetAuctionById(long auctionId)
        {
            var auction = _auctionRepository.GetById(auctionId);
            if (auction == null) { throw new NullReferenceException($"{GetType().Name} - {nameof(GetAuctionById)} auction not found."); }
            return auction;
        }

        private User GetUserById(long userId)
        {
            var seller = _userRepository.GetById(userId);
            if (seller == null) { throw new NullReferenceException($"{GetType().Name} - {nameof(GetUserById)} user not found."); }
            return seller;
        }

        private Category GetCategoryById(long categoryId)
        {
            var category = _categoryRepository.GetById(categoryId);
            if (category == null) { throw new NullReferenceException($"{GetType().Name} - {nameof(GetCategoryById)} category not found."); }
            return category;
        }
    }
}
