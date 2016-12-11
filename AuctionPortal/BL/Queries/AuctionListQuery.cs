using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using BL.AppInfrastructure;
using BL.DTOs.Auctions;
using BL.DTOs.Filters;
using DAL.Entities;
using DelegateDecompiler;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Queries
{
    public class AuctionListQuery : AppQuery<AuctionDTO>
    {
        public AuctionFilter Filter { get; set; }
        public AuctionListQuery(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        protected override IQueryable<AuctionDTO> GetQueryable()
        {
            IQueryable<Auction> query = Context.Auctions
                .Include(nameof(Auction.Seller))
                .Include(nameof(Auction.Bids))
                .Include(nameof(Auction.Category))
                //.Include(nameof(Auction.Comments))
                .Include(nameof(Auction.Items));

            if (Filter == null) return query.ProjectTo<AuctionDTO>();
            if (Filter.CategoryIds?.Any() ?? false)
            {
                query = query.Where(a => Filter.CategoryIds.Any(x=>x == a.Category.ID));
            }
            if (Filter.DeliveryOptions?.Any() ?? false)
            {
                query = query.Where(a => Filter.DeliveryOptions.Any(dop=>a.DeliveryOptionsSerialized.Contains(((int)dop).ToString())));
            }
            if (Filter.PaymentOptions?.Any() ?? false)
            {
                //query = query.Where(a => a.PaymentOptionsSerialized.Split(';').Any(pop => Filter.PaymentOptions.Any(f => ((int)f).ToString() == pop)));
                query = query.Where(a => Filter.PaymentOptions.Any(pop => a.PaymentOptionsSerialized.Contains(((int)pop).ToString())));
            }
            if (Filter.SellerId > 0)
            {
                query = query.Where(a => a.Seller.ID == Filter.SellerId);
            }
            if (Filter.MinPrice > 0)
            {
                //query = query.Where(a => a.CurrentPrice >= Filter.MinPrice);
                query = query.Where(a => (a.Bids.Count > 0 ? (a.Bids.OrderByDescending(b => b.BidTime).FirstOrDefault().Value) : a.BasePrice) >= Filter.MinPrice);
            }
            if (Filter.MaxPrice > 0)
            {

                //query = query.Where(a => a.CurrentPrice <= Filter.MaxPrice);
                query = query.Where(a => (a.Bids.Count > 0? (a.Bids.OrderByDescending(b => b.BidTime).FirstOrDefault().Value ) : a.BasePrice) <= Filter.MaxPrice);
            }
            if (Filter.StartDate.HasValue)
            {
                query = query.Where(a =>  a.AuctionEnd >= Filter.StartDate.Value);
            }
            if (Filter.EndDate.HasValue)
            {
                query = query.Where(a => a.AuctionEnd <= Filter.EndDate.Value);
            }
            if (Filter.OnlyCurrentAuctions)
            {
                query = query.Where(a => a.AuctionEnd > DateTime.Now && a.AuctionStart <= DateTime.Now && a.Delivery == null);
            }

            return query.ProjectTo<AuctionDTO>();
        }
    }
}
