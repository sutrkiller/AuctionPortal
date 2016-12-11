using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Autentization;
using AutoMapper;
using BL.DTOs.UserAccount;
using BL.DTOs.Users;
using BL.Queries;
using BL.Repositories;
using BL.Utils.Claims;
using BrockAllen.MembershipReboot;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using UserAccount = Autentization.UserAccount;

namespace BL.Services.Users
{
    public class UserService : BaseService, IUserService
    {
        public int UserPageSize => 20;

        #region Dependencies

        private readonly UserAccountRepository _userAccountRepository;
        private readonly UserRepository _userRepository;
        private readonly UserListQuery _userListQuery;
        private readonly UserByEmailQuery _userByEmailQuery;
        private readonly UserAccountService<Autentization.UserAccount> _userAccountService;

        public UserService(UserAccountRepository userAccountRepository, UserRepository userRepository, UserListQuery userListQuery, UserByEmailQuery userByEmailQuery, UserAccountService<UserAccount> userAccountService)
        {
            _userAccountRepository = userAccountRepository;
            _userRepository = userRepository;
            _userListQuery = userListQuery;
            _userByEmailQuery = userByEmailQuery;
            _userAccountService = userAccountService;
        }

        #endregion

        public void CreateUser(Guid userAccountId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var userAccount = _userAccountRepository.GetById(userAccountId);

                var user = new User() {UserAccount = userAccount};
                _userRepository.Insert(user);

                uow.Commit();
            }
        }

        public void EditUser(UserDTO userDTO)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var user = _userRepository.GetById(userDTO.ID,u=>u.UserAccount);
                Mapper.Map(userDTO, user);

                _userRepository.Update(user);

                uow.Commit();
            }
        }

        public UserDTO GetUser(long userId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var user = _userRepository.GetById(userId, u=>u.UserAccount);
                return user != null ? Mapper.Map<UserDTO>(user) : null;
            }
        }

        public UserListQueryResultDTO ListUsers(int requiredPage)
        {
            using (UnitOfWorkProvider.Create())
            {
                var query = _userListQuery;
                query.ClearSortCriterias();
                query.Skip = Math.Max(0, requiredPage - 1)*UserPageSize;
                query.Take = UserPageSize;

                query.AddSortCriteria(user=>user.LastName,SortDirection.Ascending);

                return new UserListQueryResultDTO()
                {
                    RequestedPage = requiredPage,
                    TotalResultCount = query.GetTotalRowCount(),
                    ResultPage = query.Execute()
                };
            }
        }

        public UserDTO GetUserByEmail(string email)
        {
            using (UnitOfWorkProvider.Create())
            {
                _userByEmailQuery.Email = email;
                return _userByEmailQuery.Execute().SingleOrDefault();
            }
        }

        public Guid RegisterUserAccount(UserRegistrationDTO userRegistration, bool createAdmin = false)
        {
            using (UnitOfWorkProvider.Create())
            {
                var userClaims = new List<Claim>();

                if (createAdmin)
                {
                    userClaims.Add(new Claim(ClaimTypes.Role, Claims.Admin));
                }
                else
                {
                    // for the moment there is just Customer role left
                    userClaims.Add(new Claim(ClaimTypes.Role, Claims.Authenticated));
                }

                var account = _userAccountService.CreateAccount(null, userRegistration.Password, userRegistration.Email, (Guid?)null, null);

                Mapper.Map(userRegistration, account);

                foreach (var claim in userClaims)
                {
                    _userAccountService.AddClaim(account.ID, claim.Type, claim.Value);
                }

                _userAccountService.Update(account);

                return account.ID;
            }
        }

        public void RemoveUser(long id)
        {
            using (var uw = UnitOfWorkProvider.Create())
            {
                var user = _userRepository.GetById(id);
                if (user == null) return;
                var guid = user.UserAccount.ID;
                _userRepository.Delete(id);
                _userAccountRepository.Delete(guid);
                uw.Commit();
            }
            
        }

        public Guid AuthenticateUser(UserLoginDTO loginDto)
        {
            UserAccount account;
            var result = _userAccountService.Authenticate(loginDto.Username, loginDto.Password, out account);
            if (!result)
            {
                Debug.WriteLine($"Failed to authenticate user: {loginDto.Username}");
                return Guid.Empty;
            }
            return account.ID;
        }
    }
}
