using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BL.AppInfrastructure;
using BL.DTOs.Users;
using Castle.Core.Internal;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Queries
{
    public class UserByEmailQuery : AppQuery<UserDTO>
    {
        public string Email { get; set; }
        public UserByEmailQuery(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        protected override IQueryable<UserDTO> GetQueryable()
        {
            if (string.IsNullOrWhiteSpace(Email) || !new EmailAddressAttribute().IsValid(Email))
            {
                throw new InvalidOperationException($"{GetType().Name} - {nameof(Email)} must be valid.");
            }

            var user = Context.Users.Include(nameof(User.UserAccount))
                .FirstOrDefault(u => Email.Equals(u.UserAccount.Email));

            if (user == null)
            {
                return new EnumerableQuery<UserDTO>(new List<UserDTO>());
            }

            var userDto = Mapper.Map<UserDTO>(user);
            return new EnumerableQuery<UserDTO>(new List<UserDTO>() {userDto});

        }
    }
}
