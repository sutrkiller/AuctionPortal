using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using BL.AppInfrastructure;
using BL.DTOs.Items;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Queries
{
    public class ItemListQuery : AppQuery<ItemDTO>
    {
        public long AuctionId { get; set; }

        public ItemListQuery(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        protected override IQueryable<ItemDTO> GetQueryable()
        {
            IQueryable<Item> query = Context.Items.Include(nameof(Item.ItemImages)).Include(nameof(Item.Auction));

            if (AuctionId > 0)
            {
                query = query.Where(x => x.Auction.ID == AuctionId);
            }

            return query
                .ProjectTo<ItemDTO>();
        }
    }
}
