using System;
using System.Security.Claims;
using System.Security.Principal;
using BL.DTOs.UserAccount;
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

        public int UserPageSize => _userService.UserPageSize;

        public void CreateUser(Guid userAccountId)
        {
            if (userAccountId == null) throw new ArgumentException("UserAccount must exist prior to the creation of user.");
            _userService.CreateUser(userAccountId);
        }

        public void EditUser(UserDTO userDTO)
        {
            if (userDTO.ID <= 0) throw new FormatException($"User with id {userDTO.ID} does not exist.");
            if (string.IsNullOrWhiteSpace(userDTO.Email)) throw new FormatException($"Email cannot be empty.");
            _userService.EditUser(userDTO);
        }

        public UserDTO GetUser(long userId)
        {
            if (userId <= 0) throw new FormatException($"User with id {userId} does not exist.");
            return _userService.GetUser(userId);
        }

        

        public void DeleteUser(long id)
        {
            _userService.RemoveUser(id);
        }

        public UserListQueryResultDTO ListUsers(int requiredPage = 1)
        {
            return _userService.ListUsers(requiredPage);
        }

        public UserDTO GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException($"Email cannot be empty.");
            return _userService.GetUserByEmail(email);
        }

        public Guid RegisterUser(UserRegistrationDTO registrationDto, out bool success)
        {
            if (_userService.GetUserByEmail(registrationDto.Email) != null)
            {
                success = false;
                return new Guid();
            }
            var accountId = _userService.RegisterUserAccount(registrationDto);
            _userService.CreateUser(accountId);
            success = true;
            return accountId;
        }

        /// <summary>
        /// Authenticates user with given username and password
        /// </summary>
        /// <param name="loginDto">user login details</param>
        /// <returns>ID of the authenticated user</returns>
        public Guid AuthenticateUser(UserLoginDTO loginDto)
        {
            return _userService.AuthenticateUser(loginDto);
        }

        public UserDTO GetUser(IIdentity identity)
        {
            var email = (identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Email)?.Value;
            return string.IsNullOrEmpty(email) ? null : _userService.GetUserByEmail(email);
        }
    }
}
