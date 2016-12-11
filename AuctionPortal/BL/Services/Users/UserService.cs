using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autentization;
using AutoMapper;
using BL.DTOs.Users;
using BL.Queries;
using BL.Repositories;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;

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

        public UserService(UserAccountRepository userAccountRepository, UserRepository userRepository, UserListQuery userListQuery, UserByEmailQuery userByEmailQuery)
        {
            _userAccountRepository = userAccountRepository;
            _userRepository = userRepository;
            _userListQuery = userListQuery;
            _userByEmailQuery = userByEmailQuery;
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
    }
}
