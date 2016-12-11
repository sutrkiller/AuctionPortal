using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BL.DTOs.Users;
using BL.Services.Users;

namespace BL.Facades
{
    public class UserFacade
    {
        #region Dependencies

        private readonly IUserService _userService;

        public UserFacade(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        private int UserPageSize => _userService.UserPageSize;

        void CreateUser(Guid userAccountId)
        {
            if (userAccountId == null) throw new ArgumentException("UserAccount must exist prior to the creation of user.");
            _userService.CreateUser(userAccountId);
        }

        void EditUser(UserDTO userDTO)
        {
            if(userDTO.ID <= 0) throw new FormatException($"User with id {userDTO.ID} does not exist.");
            if(string.IsNullOrWhiteSpace(userDTO.Email)) throw new FormatException($"Email cannot be empty.");
            _userService.EditUser(userDTO);
        }

        UserDTO GetUser(long userId)
        {
            if (userId <= 0) throw new FormatException($"User with id {userId} does not exist.");
            return _userService.GetUser(userId);
        }

        UserListQueryResultDTO ListUsers(int requiredPage =1)
        {
            return _userService.ListUsers(requiredPage);
        }

        UserDTO GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException($"Email cannot be empty.");
            return _userService.GetUserByEmail(email);
        }
    }
}
