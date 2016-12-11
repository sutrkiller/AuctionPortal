using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTOs.UserAccount;
using BL.DTOs.Users;

namespace BL.Services.Users
{
    public interface IUserService
    {
        int UserPageSize { get;  }

        void CreateUser(Guid userAccountId);

        void EditUser(UserDTO userDTO);
        void RemoveUser(long id);
        UserDTO GetUser(long userId);

        UserListQueryResultDTO ListUsers(int requiredPage);

        UserDTO GetUserByEmail(string email);

        Guid RegisterUserAccount(UserRegistrationDTO userRegistration, bool createAdmin = false);

        Guid AuthenticateUser(UserLoginDTO loginDto);
    }
}
