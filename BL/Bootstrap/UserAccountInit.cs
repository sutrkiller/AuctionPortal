using System;
using BL.DTOs.UserAccount;
using BL.Facades;
using BL.Services.Users;
using Castle.Windsor;

namespace BL.Bootstrap
{
    public class UserAccountInit
    {
        /// <summary>
        /// Initializes DB with various user accounts and promo codes
        /// </summary>
        /// <param name="container"></param>
        public static void InitializeUserAccounts(IWindsorContainer container)
        {
            CreateUsers(container);
        }

        /// <summary>
        /// Creates users (admin and customers) for demo eshop
        /// </summary>
        /// <param name="container">The windsor container</param>
        private static void CreateUsers(IWindsorContainer container)
        {
            var userAccountManagementService = container.Resolve<IUserService>();
            var customerFacade = container.Resolve<UserFacade>();
            bool success;

            userAccountManagementService.RegisterUserAccount(new UserRegistrationDTO
            {
                Address = "Kohoutovice, Brno, Czech Republic",
                BirthDate = new DateTime(1993, 1, 1),
                Email = "admin@auctionPortal.com",
                FirstName = "Admin",
                LastName = "Auction",
                Password = "admin"
            }, true);

            customerFacade.RegisterUser(new UserRegistrationDTO
            {
                Address = "Nekdejinde, Brno, Czech Republic",
                BirthDate = new DateTime(1985, 8, 20),
                Email = "john@comp.com",
                FirstName = "John",
                LastName = "Whoever",
                Password = "Password" // same for the email account
            }, out success);

            customerFacade.RegisterUser(new UserRegistrationDTO
            {
                Address = "SomeplaceElse, Brno, Czech Republic",
                BirthDate = new DateTime(1985, 11, 6),
                Email = "emily@comp.com",
                FirstName = "Emily",
                LastName = "Nobody",
                Password = "Password" // same for the email account
            }, out success);
        }
    }
}
