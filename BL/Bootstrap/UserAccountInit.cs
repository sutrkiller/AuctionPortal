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
                Address = "Monte Bu, Brno, Czech Republic",
                BirthDate = new DateTime(1980, 1, 1),
                Email = "admin@demoEshop.com",
                FirstName = "DemoEshop",
                LastName = "Administrator",
                Password = "admin1234"
            }, true);

            customerFacade.RegisterUser(new UserRegistrationDTO
            {
                Address = "No Name Street, Turnersville, New Jersey",
                BirthDate = new DateTime(1985, 8, 20),
                Email = "sophieTurnerDemoEshop@zoho.com",
                FirstName = "Sophie",
                LastName = "Turner",
                Password = "SecretPa$$" // same for the email account
            }, out success);

            customerFacade.RegisterUser(new UserRegistrationDTO
            {
                Address = "No Name Street, Turnersville, New Jersey",
                BirthDate = new DateTime(1985, 11, 6),
                Email = "alfieAllenDemoEshop@zoho.com", // password: SecretPa$$
                FirstName = "Alfie",
                LastName = "Allen",
                Password = "SecretPa$$" // same for the email account
            }, out success);
        }
    }
}
