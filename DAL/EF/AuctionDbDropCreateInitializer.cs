using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Ef;
using DAL.Entities;
using UserAccount = Autentization.UserAccount;

namespace DAL.EF
{
    public class AuctionDbDropCreateInitializer : DropCreateDatabaseAlways<AuctionDbContext>
    {
        public override void InitializeDatabase(AuctionDbContext context)
        {
            base.InitializeDatabase(context);
        }

        protected override void Seed(AuctionDbContext context)
        {
            var category = new Category() { Name = "Electronics" };
            var cat1 = new Category() { Name = "Books" };
            context.Categories.Add(cat1);

            var us = new UserAccountService<UserAccount>(new DbContextUserAccountRepository<AuctionDbContext, UserAccount>(context));
            var ua = us.CreateAccount("Tobias", "heslo", "email@prov.com",null,null,new [] {new Claim(ClaimTypes.Role, "Authenticated") });
            ua.FirstName = "Tobias";
            ua.LastName = "Kamenicky";
            ua.Address = "Nove Mesto na Morave, Czech Republic";
            var user = new User()
            {
                UserAccount = ua
            };

            var auction = new Auction()
            {
                AuctionStart = DateTime.Now.AddDays(-7),
                AuctionEnd = DateTime.Now.AddMinutes(2),
                BasePrice = 2000m,
                Category = category,
                DeliveryOptionsSerialized = "1",
                PaymentOptionsSerialized = "0",
                Seller = user,
                Items = new List<Item>() { new Item()
                {
                    Name = "Nexus 5X",
                    Description = "This is amazing phone that will blow your mind.",
                    ItemImages = new List<ItemImage> { new ItemImage { ImagePath = @"~\Content\Images\Tmp\nexus5x_2.jpg" }, new ItemImage { ImagePath = @"~\Content\Images\Tmp\nexus5x.jpeg" } }
                } }
            };
            var auction2 = new Auction()
            {
                AuctionStart = DateTime.Now.AddDays(-5),
                AuctionEnd = DateTime.Now.AddDays(-1),
                BasePrice = 3000m,
                Category = cat1,
                DeliveryOptionsSerialized = "0",
                PaymentOptionsSerialized = "0;1",
                Seller = user,
                Items = new List<Item>() { new Item()
                {
                    Name = "Mastering C# and .NET Programming",
                     Description = "Mastering C# and .NET Programming will take you in to the depths of C# 6.0 and .NET 4.6, so you can understand how the platform works when it runs your code, and how you can use this knowledge to write efficient applications. Take full advantage of the new revolution in .NET development, including open source status and cross-platform capability, and get to grips with the architectural changes of CoreCLR. Start with how the CLR executes code, and discover the niche and advanced aspects of C# programming – from delegates and generics, through to asynchronous programming. Run through new forms of type declarations and assignments, source code callers, static using syntax, auto-property initializers, dictionary initializers, null conditional operators, and many others. Then unlock the true potential of the .NET platform. Learn how to write OWASP-compliant applications, how to properly implement design patterns in C#, and how to follow the general SOLID principles and its implementations in C# code. We finish by focusing on tips and tricks that you'll need to get the most from C# and .NET.",
                     ItemImages = new List<ItemImage> {new ItemImage { ImagePath = @"~\Content\Images\Tmp\csharpbook.jpg" } }
                } },

            };
            context.Auctions.Add(auction);

            var comments = new List<Comment>
            {
                new Comment()
                {
                    Author = user,
                    Auction = auction,
                    Text = "How much memory does this phone have?",
                    Time = DateTime.Now,
                }
            };
            context.Comments.AddRange(comments);
            context.Auctions.Add(auction2);
            context.SaveChanges();
            base.Seed(context);
        }
    }
}
