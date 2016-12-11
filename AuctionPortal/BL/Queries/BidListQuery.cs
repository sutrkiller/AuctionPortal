using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using BL.AppInfrastructure;
using BL.DTOs.Bids;
using BL.DTOs.Filters;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Queries
{
    public class BidListQuery : AppQuery<BidDTO>
    {
        public BidFilter Filter { get; set; }
        public BidListQuery(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        protected override IQueryable<BidDTO> GetQueryable()
        {
            IQueryable<Bid> query = Context.Bids.Include(nameof(Bid.Auction));

            if (Filter == null) return query.ProjectTo<BidDTO>();
            if (Filter.AuctionId.HasValue && Filter.AuctionId > 0)
            {
                query = query.Where(q => q.Auction.ID == Filter.AuctionId.Value);
            }
            if (Filter.StartTime.HasValue)
            {
                query = query.Where(q => q.BidTime >= Filter.StartTime);
            }
            if (Filter.EndTime.HasValue)
            {
                query = query.Where(q => q.BidTime <= Filter.EndTime);
            }
            return query.ProjectTo<BidDTO>();
        }
    }
}
