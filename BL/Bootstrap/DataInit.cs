using System;
using System.Collections.Generic;
using System.Linq;
using BL.DTOs.Auctions;
using BL.DTOs.Categories;
using BL.DTOs.Comments;
using BL.DTOs.ItemImages;
using BL.DTOs.Items;
using BL.DTOs.UserAccount;
using BL.Facades;
using Castle.Windsor;
using DAL.Enums;

namespace BL.Bootstrap
{
    public class DataInit
    {
        public static void InitializeDb(IWindsorContainer container)
        {
            InitializeCategories(container);
            InitializeUser(container);
            InitializeAuctions(container);
            InitializeComment(container);
        }

        private static void InitializeComment(IWindsorContainer container)
        {
            var auctionFacade = container.Resolve<AuctionFacade>();
            var userFacade = container.Resolve<UserFacade>();

            var auction = auctionFacade.GetAuctions(null)?.ResultPage?.SingleOrDefault(x => x.BasePrice == 2000m);
            if (auction == null) return;
            auctionFacade.CreateComment(
                new CommentCreateDTO
                {
                      AuctionId = auction.ID,
                      AuthorId = userFacade.GetUserByEmail("john@comp.com").ID,
                      Text = "How much memory does this phone have?",
                      Time = DateTime.Now
                });
        }

        private static void InitializeAuctions(IWindsorContainer container)
        {
            var auctionFacade = container.Resolve<AuctionFacade>();
            var userFacade = container.Resolve<UserFacade>();

            var catEl = auctionFacade.GetCategoryId("Electronics");
            var catBooks = auctionFacade.GetCategoryId("Books");
            var user = userFacade.GetUserByEmail("email@prov.com").ID;
            auctionFacade.CreateAuction(new AuctionCreateDTO
            {
                AuctionStart = DateTime.Now.AddDays(-7),
                AuctionEnd = DateTime.Now.AddMinutes(20),
                BasePrice = 2000m,
                CategoryId = catEl,
                DeliveryOptions = new[] { DeliveryType.Post },
                PaymentOptions = new[] { PaymentMethod.Card, PaymentMethod.BankTransfer },
                SellerId = user,
            }, new List<ItemCreateDTO>
            {
                new ItemCreateDTO
                {
                    Item = new ItemDTO
                    {
                         Name = "Nexus 5X",
                         Description = "This is amazing phone that will blow your mind."
                    },
                    Images = new List<ItemImageDTO>
                    {
                        new ItemImageDTO
                        {
                            ImagePath = @"~\Content\Images\Tmp\nexus5x_2.jpg"
                        },
                        new ItemImageDTO
                        {
                            ImagePath = @"~\Content\Images\Tmp\nexus5x.jpeg"
                        }
                    }
                }
            });

            auctionFacade.CreateAuction(new AuctionCreateDTO
            {
                AuctionStart = DateTime.Now.AddDays(-5),
                AuctionEnd = DateTime.Now.AddDays(-1),
                BasePrice = 3000m,
                CategoryId = catBooks,
                DeliveryOptions = new[] { DeliveryType.Post,DeliveryType.PersonalCollection,  },
                PaymentOptions = new[] { PaymentMethod.Cash },
                SellerId = user,
            }, new List<ItemCreateDTO>
            {
                new ItemCreateDTO
                {
                    Item = new ItemDTO
                    {
                         Name = "Mastering C# and .NET Programming",
                         Description = "Mastering C# and .NET Programming will take you in to the depths of C# 6.0 and .NET 4.6, so you can understand how the platform works when it runs your code, and how you can use this knowledge to write efficient applications. Take full advantage of the new revolution in .NET development, including open source status and cross-platform capability, and get to grips with the architectural changes of CoreCLR. Start with how the CLR executes code, and discover the niche and advanced aspects of C# programming – from delegates and generics, through to asynchronous programming. Run through new forms of type declarations and assignments, source code callers, static using syntax, auto-property initializers, dictionary initializers, null conditional operators, and many others. Then unlock the true potential of the .NET platform. Learn how to write OWASP-compliant applications, how to properly implement design patterns in C#, and how to follow the general SOLID principles and its implementations in C# code. We finish by focusing on tips and tricks that you'll need to get the most from C# and .NET."
                    },
                    Images = new List<ItemImageDTO>
                    {
                        new ItemImageDTO
                        {
                           ImagePath = @"~\Content\Images\Tmp\csharpbook.jpg"
                        }
                    }
                }
            });
        }

        private static void InitializeUser(IWindsorContainer container)
        {
            var userFacade = container.Resolve<UserFacade>();

            bool success;
            userFacade.RegisterUser(new UserRegistrationDTO
            {
                Address = "Nove Mesto na Morave, Czech Republic",
                BirthDate = new DateTime(1993, 1, 1),
                Email = "email@prov.com",
                FirstName = "Tobias",
                LastName = "Kamenicky",
                Password = "heslo"
            }, out success);
        }

        private static void InitializeCategories(IWindsorContainer container)
        {
            var auctionFacade = container.Resolve<AuctionFacade>();

            auctionFacade.CreateCategory(new CategoryDTO { Name = "Electronics" });
            auctionFacade.CreateCategory(new CategoryDTO { Name = "Books" });
        }
    }
}
