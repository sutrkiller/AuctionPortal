using System;
using System.Diagnostics;
using System.Linq;
using BL.Bootstrap;
using BL.DTOs.Auctions;
using BL.DTOs.Bids;
using BL.DTOs.Categories;
using BL.DTOs.Filters;
using BL.DTOs.Items;
using BL.Services.Auctions;
using BL.Services.Categories;
using BL.Services.Items;
using BL.Services.Users;
using BL.Utils.Enums;
using BrockAllen.MembershipReboot;
using Castle.Windsor;
using DAL.Enums;
using UserAccount = Autentization.UserAccount;

namespace TestingApp
{
    public class Program
    {
        private static IAuctionService _auctionService;
        private static ICategoryService _categoryService;
        private static IItemService _itemService;
        private static IUserService _userService;
        private static UserAccountService<UserAccount> _userAccountService;

        private static readonly IWindsorContainer Container = new WindsorContainer();

        private static long _catId;
        private static long _usrId;
        private static long _aucId;

        static void Main()
        {
            InitializeWindsorContainerAndMapper();
            TestCategoryService();
            Console.WriteLine(@"Category tests successful.");
            TestUserService();
            Console.WriteLine(@"User tests successful.");
            TestAuctionService();
            Console.WriteLine(@"Auction tests successful.");
            TestItemService();
            Console.WriteLine(@"Item tests successful.");

            Console.WriteLine(@"All tests successful.");
            Console.ReadKey();
        }

        private static void TestAuctionService()
        { 
            _auctionService = Container.Resolve<IAuctionService>();

            _auctionService.CreateAuction(new AuctionCreateDTO()
            {
                AuctionStart = DateTime.Now,
                AuctionEnd = DateTime.Now.AddDays(7),
                BasePrice = 1000,
                CategoryId = _catId,
                DeliveryOptions = new[] {DeliveryType.Post},
                PaymentOptions = new[] {PaymentMethod.Card, PaymentMethod.Cash },
                SellerId = _usrId
            });

            var aucs = _auctionService.GetAuctions(new AuctionFilter() {MaxPrice = 999});
            Debug.Assert(aucs.TotalResultCount == 0);

            aucs = _auctionService.GetAuctions(new AuctionFilter());
            Debug.Assert(aucs.TotalResultCount == 1 && aucs.ResultPage.All(a=>a.ID > 0));

            _aucId = aucs.ResultPage.First().ID;

            var minBid =_auctionService.GetMinimumBid(_aucId);
            Debug.Assert(minBid == 1000);

            var oneClick = _auctionService.GetOneClickBuyPrice(_aucId);
            Debug.Assert(oneClick == new decimal(1000 * _auctionService.OneClickBuyMultiplier));

            _auctionService.CreateBid(new BidDTO {AuctionId = _aucId,BidderId = _usrId,BidTime = DateTime.Now,Value = 1000m});
            minBid = _auctionService.GetMinimumBid(_aucId);
            Debug.Assert(minBid == new decimal(10*(100+_auctionService.MinBidRisePercentage)));

            var auc = _auctionService.GetAuction(_aucId, false);
            Debug.Assert(auc!=null);

            _auctionService.CreateAuction(new AuctionCreateDTO()
            {
                AuctionStart = DateTime.Now,
                AuctionEnd = DateTime.Now,
                BasePrice = 1001,
                CategoryId = _catId,
                DeliveryOptions = new[] { DeliveryType.PersonalCollection },
                PaymentOptions = new[] { PaymentMethod.Card },
                SellerId = _usrId
            });

            aucs = _auctionService.GetAuctions(new AuctionFilter()
            {
                CategoryIds = new[] {auc.CategoryId},
                DeliveryOptions = new[] {DeliveryType.PersonalCollection, DeliveryType.Post },
                PaymentOptions = new[] {auc.PaymentOptions[0]},
                StartDate = auc.AuctionStart.AddDays(-1),
                EndDate = auc.AuctionEnd.AddDays(1),
                MaxPrice = 1001,
                MinPrice = 1000,
                SellerId = auc.SellerId,
                SortCriteria = AuctionSortCriteria.AuctionStart,
                SortAscending = false
            });

            Debug.Assert(aucs.TotalResultCount == 2);
            Debug.Assert(aucs.ResultPage.First().BasePrice == 1000);
            Debug.Assert(aucs.ResultPage.Last().BasePrice == 1001);

            Debug.Assert(_auctionService.GetAuctions(new AuctionFilter() {OnlyCurrentAuctions = true}).TotalResultCount == 1);

            _auctionService.CreateBid(new BidDTO() {AuctionId = _aucId,BidderId = _usrId,BidTime = DateTime.Now,Value = minBid});

            Debug.Assert(_auctionService.GetAuction(_aucId, false).CurrentPrice == minBid);
            _auctionService.CreateBid(new BidDTO() { AuctionId = _aucId, BidderId = _usrId, BidTime = DateTime.Now, Value = _auctionService.GetMinimumBid(_aucId) });

            var bids = _auctionService.GetBids(new BidFilter() {SortAscending = true});
            Debug.Assert(bids.TotalResultCount == 3);
            Debug.Assert(bids.ResultPage.First().Value < bids.ResultPage.Last().Value);

        }
        
        private static void TestUserService()
        {
            _userAccountService = new UserAccountService<UserAccount>(Container.Resolve<IUserAccountRepository<UserAccount>>());
            
            _userService = Container.Resolve<IUserService>();

            //_userAccountService.
            UserAccount account = _userAccountService.CreateAccount("tobias", "blabla", "tobias.kamenicky@gmail.com");
            Debug.Assert(account.ID != null);

            _userService.CreateUser(account.ID);
            var user = _userService.GetUserByEmail("tobias.kamenicky@gmail.com");
            Debug.Assert(user != null && user.ID > 0);

            user.FirstName = "Tobias";
            _userService.EditUser(user);
            user.LastName = "Kamenicky";
            _userService.EditUser(user);
            var user2 = _userService.GetUser(user.ID);
            Debug.Assert(user2 != null && user2.FirstName == "Tobias" && user2.LastName == "Kamenicky");

            _usrId = user2.ID;
        }

        private static void TestItemService()
        {
            _itemService = Container.Resolve<IItemService>();

            _itemService.CreateItem(new ItemDTO() {Name = "Item",AuctionId = _aucId,Description = "Something pretty amazing worth your money."});

            var items = _itemService.GetAllItems(_aucId).ToList();
            Debug.Assert(items.Count == 1);
            Debug.Assert(items[0].ID > 0);
            var itemId = items[0].ID;

            _itemService.EditItem(new ItemDTO() {AuctionId = _aucId,Description = "Something else",ID = itemId,Name = "Item2"});
            var item = _itemService.GetItem(itemId);
            Debug.Assert(item.Name == "Item2");
            Debug.Assert(_itemService.GetAllItems().Count() == 1);

            _itemService.DeleteItem(itemId);
            Debug.Assert(!_itemService.GetAllItems().Any());
            Debug.Assert(!_itemService.GetItemImages(itemId).Any());
        }

        private static void InitializeWindsorContainerAndMapper()
        {
            Container.Install(new BusinessLayerInstaller());
            MappingInit.ConfigureMapping();
        }

        private static void TestCategoryService()
        {
            _categoryService = Container.Resolve<ICategoryService>();

            _categoryService.CreateCategory(new CategoryDTO() { Name = "Old stuff" });
            _catId = _categoryService.GetCategoryId("Old stuff");
            Debug.Assert(_catId>0);

            _categoryService.EditCategory(new CategoryDTO() { ID = _catId, Name = "New stuff" });
            string categoryName = _categoryService.GetCategory(_catId).Name;
            Debug.Assert(categoryName == "New stuff");

            _categoryService.CreateCategory(new CategoryDTO() { Name = "Second" });
            _categoryService.DeleteCategory(_catId);
            var all = _categoryService.GetAllCategories().ToList();
            Debug.Assert(all.Count == 1);
            Debug.Assert(all[0].Name == "Second");

            _catId = all[0].ID;
        }
    }
}
