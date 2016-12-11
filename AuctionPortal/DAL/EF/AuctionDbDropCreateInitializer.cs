using System;
using System.Collections.Generic;
using System.Data.Entity;
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
           var category = new Category() {Name = "Testing category"};
            var cat1 = new Category() {Name = "Category 2"};
            context.Categories.Add(cat1);
            var us = new UserAccountService<UserAccount>(new DbContextUserAccountRepository<AuctionDbContext,UserAccount>(context));
            var ua = us.CreateAccount("name", "blabla", "email@prov.com");
            ua.FirstName = "Tobias";
            ua.LastName = "Kamenicky";
            var user = new User()
            {
                UserAccount = ua
            };
            var auction = new Auction()
            {
                AuctionStart = new DateTime(2016,11,15),
                AuctionEnd = new DateTime(2016,11,25),
                BasePrice = 2000m,
                Category = category,
                DeliveryOptionsSerialized = "1",
                PaymentOptionsSerialized = "0",
                Seller = user,
                Items = new List<Item>() { new Item()
                {
                    Name = "Item 1",
                    Description = "This is very long description about the greatness of this item. Buying is definitely worth it because none shell see such a great item again. Bla bla.."
                } }
            };
            var auction2 = new Auction()
            {
                AuctionStart = DateTime.Now.AddDays(-5),
                AuctionEnd = DateTime.Now.AddDays(-1),
                BasePrice = 3000m,
                Category = category,
                DeliveryOptionsSerialized = "0",
                PaymentOptionsSerialized = "0;1",
                Seller = user,
                Items = new List<Item>() { new Item()
                {
                    Name = "Item 2",
                     Description = "Short description"
                } },
                
            };
            context.Auctions.Add(auction);
            var comments = new List<Comment>
            {
                new Comment()
                {
                    Author = user,
                    Auction = auction,
                    Text = "Some pretty amazing comment",
                    Time = DateTime.Now,
                    ChildComments = new List<Comment>
                    {
                        new Comment
                        {
                            Author = user,
                            Auction = auction,
                            Text = "Reply to amazing comment",
                            Time = DateTime.Now.AddSeconds(20)
                        }
                    }
                }
            };
            context.Comments.AddRange(comments);
            context.Auctions.Add(auction2);
            context.SaveChanges();
            base.Seed(context);
        }
    }
}
