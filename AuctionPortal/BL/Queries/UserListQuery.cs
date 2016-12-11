using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using BL.AppInfrastructure;
using BL.DTOs.Users;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Queries
{
    public class UserListQuery : AppQuery<UserDTO>
    {
        public UserListQuery(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        protected override IQueryable<UserDTO> GetQueryable()
        {
            return Context.Users//.Include(nameof(User.Address))
                .Include(nameof(User.UserAccount))
                .Include(nameof(User.Auctions))
                .Include(nameof(User.Bids))
                .Include(nameof(User.BoughtItemsDeliveries))
                .Include(nameof(User.Comments))
                .Include(nameof(User.SoldItemsDeliveries))
                .ProjectTo<UserDTO>();
        }
    }
}
