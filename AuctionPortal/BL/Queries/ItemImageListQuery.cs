using System.Linq;
using AutoMapper.QueryableExtensions;
using BL.AppInfrastructure;
using BL.DTOs.ItemImages;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Queries
{
    public class ItemImageListQuery : AppQuery<ItemImageDTO>
    {
        public long ItemId { get; set; }

        public ItemImageListQuery(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        protected override IQueryable<ItemImageDTO> GetQueryable()
        {
            IQueryable<ItemImage> query = Context.ItemImages;
            if (ItemId>0)
            {
                query = query.Where(ii => ii.Item.ID == ItemId);
            }

            return query.ProjectTo<ItemImageDTO>();
        }
    }
}
