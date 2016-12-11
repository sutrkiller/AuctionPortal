using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Autentization;
using DAL.Entities;

namespace DAL.EF
{
    public class AuctionDbContext : DbContext
    {
        public AuctionDbContext() : base("AuctionDB")
        {
           InitializeDb();
        }

        public AuctionDbContext(string connectionName) : base(connectionName)
        {
            InitializeDb();
        }

        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<ItemImage> ItemImages { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureMembershipRebootUserAccounts<UserAccount>();

            modelBuilder.Entity<Auction>().HasKey(a => a.ID);
            modelBuilder.Entity<Auction>().Property(a => a.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Auction>().HasRequired(a => a.Category).WithMany(c => c.Auctions).WillCascadeOnDelete(true);
            modelBuilder.Entity<Auction>().HasRequired(a => a.Seller).WithMany(s => s.Auctions).WillCascadeOnDelete(true);
            modelBuilder.Entity<Auction>().HasOptional(a => a.Delivery).WithOptionalPrincipal(d => d.Auction).WillCascadeOnDelete(true);
            modelBuilder.Entity<Auction>().HasMany(a => a.Bids).WithRequired(b => b.Auction).WillCascadeOnDelete(false);
            modelBuilder.Entity<Auction>().HasMany(a => a.Comments).WithRequired(c => c.Auction).WillCascadeOnDelete(true);
            modelBuilder.Entity<Auction>().HasMany(a => a.Items).WithRequired(c => c.Auction).WillCascadeOnDelete(true);
            modelBuilder.Entity<Auction>().Property(a => a.AuctionStart).HasColumnType("datetime2");
            modelBuilder.Entity<Auction>().Property(a => a.AuctionEnd).HasColumnType("datetime2");

            modelBuilder.Entity<Bid>().HasKey(a => a.ID);
            modelBuilder.Entity<Bid>().Property(a => a.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Bid>().HasRequired(b => b.User).WithMany(u => u.Bids);
            modelBuilder.Entity<Bid>().Property(b => b.BidTime).HasColumnType("datetime2");

            modelBuilder.Entity<Category>().HasKey(a => a.ID);
            modelBuilder.Entity<Category>().Property(a => a.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Comment>().HasKey(a => a.ID);
            modelBuilder.Entity<Comment>().Property(a => a.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Comment>().HasRequired(c=>c.Author).WithMany(a=>a.Comments).WillCascadeOnDelete(false);
            modelBuilder.Entity<Comment>().HasMany(c=>c.ChildComments).WithOptional(c=>c.ParentComment).WillCascadeOnDelete(false);

            modelBuilder.Entity<Delivery>().HasKey(a => a.ID);
            modelBuilder.Entity<Delivery>().Property(a => a.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Delivery>().HasRequired(d=>d.Buyer).WithMany(u=>u.BoughtItemsDeliveries).WillCascadeOnDelete(false);
            modelBuilder.Entity<Delivery>().HasRequired(d=>d.Seller).WithMany(u=>u.SoldItemsDeliveries).WillCascadeOnDelete(false);

            modelBuilder.Entity<Item>().HasKey(a => a.ID);
            modelBuilder.Entity<Item>().Property(a => a.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Item>().HasMany(i => i.ItemImages).WithRequired(ii => ii.Item);

            modelBuilder.Entity<ItemImage>().HasKey(a => a.ID);
            modelBuilder.Entity<ItemImage>().Property(a => a.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<User>().HasKey(a => a.ID);
            modelBuilder.Entity<User>().Property(a => a.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<User>().HasRequired(u => u.UserAccount);




            //modelBuilder.Entity<Address>().HasKey(a => a.ID);
            //modelBuilder.Entity<Address>().Property(a=>a.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            ////modelBuilder.Entity<Address>().HasMany(a=>a.Users).WithRequired(u=>u.Address).HasForeignKey(u=>u.AddressId).WillCascadeOnDelete(false);
            //modelBuilder.Entity<Address>().Property(a => a.Street).IsRequired();
            //modelBuilder.Entity<Address>().Property(a => a.City).IsRequired();
            //modelBuilder.Entity<Address>().Property(a => a.Postcode).IsRequired();
            //modelBuilder.Entity<Address>().Property(a => a.Country).IsRequired();

            //modelBuilder.Entity<Auction>().HasKey(a => a.ID);
            //modelBuilder.Entity<Auction>().Property(a=>a.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //modelBuilder.Entity<Auction>().HasRequired(a=>a.Seller).WithMany(u=>u.Auctions).HasForeignKey(a=>a.SellerId).WillCascadeOnDelete(false);
            //modelBuilder.Entity<Auction>().HasOptional(a => a.Delivery).WithRequired(d => d.Auction);
            //modelBuilder.Entity<Auction>().HasMany(a => a.Items).WithRequired(i => i.Auction);
            //modelBuilder.Entity<Auction>().HasMany(a => a.Bids).WithRequired(b => b.Auction).HasForeignKey(i => i.AuctionId).WillCascadeOnDelete(true);
            //modelBuilder.Entity<Auction>().HasRequired(a => a.Category).WithMany(b => b.Auctions);
            //modelBuilder.Entity<Auction>().HasMany(a=>a.Comments).WithRequired(c=>c.Auction).WillCascadeOnDelete(true);
            //modelBuilder.Entity<Auction>().Property(a => a.AuctionStart).IsRequired();
            //modelBuilder.Entity<Auction>().Property(a => a.AuctionEnd).IsRequired();
            //modelBuilder.Entity<Auction>().Property(a => a.BasePrice).IsRequired();
            //modelBuilder.Entity<Auction>().Property(a => a.AuctionViews).IsRequired();
            //modelBuilder.Entity<Auction>().Property(a => a.DeliveryOptionsSerialized).IsRequired();
            //modelBuilder.Entity<Auction>().Property(a => a.PaymentOptionsSerialized).IsRequired();
            //modelBuilder.Entity<Auction>().Ignore(a => a.DeliveryOptions);
            //modelBuilder.Entity<Auction>().Ignore(a => a.PaymentOptions);
            //modelBuilder.Entity<Auction>().Ignore(a => a.Ended);
            //modelBuilder.Entity<Auction>().Ignore(a => a.CurrentPrice);

            //modelBuilder.Entity<Bid>().HasKey(b => b.ID);
            //modelBuilder.Entity<Bid>().Property(b => b.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //modelBuilder.Entity<Bid>().HasRequired(b => b.User).WithMany(u=>u.Bids).HasForeignKey(b=>b.BidderId);
            //modelBuilder.Entity<Bid>().Property(b => b.BidTime).IsRequired();
            //modelBuilder.Entity<Bid>().Property(b => b.Value).IsRequired();

            //modelBuilder.Entity<Item>().HasKey(i => i.ID);
            //modelBuilder.Entity<Item>().Property(i => i.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //modelBuilder.Entity<Item>().Property(i => i.Name).IsRequired();

            //modelBuilder.Entity<User>().HasKey(u => u.ID);
            //modelBuilder.Entity<User>().Property(u => u.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            ////modelBuilder.Entity<User>().HasRequired(u => u.UserAccount);
            //modelBuilder.Entity<User>().HasMany(u => u.Comments).WithRequired(c => c.Author).WillCascadeOnDelete(true);

            //modelBuilder.Entity<Category>().HasKey(c => c.ID);
            //modelBuilder.Entity<Category>().Property(c => c.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //modelBuilder.Entity<Category>().Property(c => c.Name).IsRequired();

            //modelBuilder.Entity<Delivery>().HasKey(d => d.ID);
            //modelBuilder.Entity<Delivery>().Property(d => d.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            ////modelBuilder.Entity<Delivery>().HasRequired(d => d.DeliveryAddress).WithMany(a=>a.Deliveries).HasForeignKey(d=>d.DeliveryAddressId);
            //modelBuilder.Entity<Delivery>().HasRequired(d => d.Seller).WithMany(a=>a.SoldItemsDeliveries).HasForeignKey(d=>d.SellerId).WillCascadeOnDelete(false);
            //modelBuilder.Entity<Delivery>().HasRequired(d => d.Buyer).WithMany(a=>a.BoughtItemsDeliveries).HasForeignKey(d=>d.BuyerId);

            //modelBuilder.Entity<ItemImage>().HasKey(i => i.ID);
            //modelBuilder.Entity<ItemImage>().Property(i => i.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //modelBuilder.Entity<ItemImage>().HasRequired(i => i.Item).WithMany(ai => ai.ItemImages);
            //modelBuilder.Entity<ItemImage>().Property(i => i.Image).IsRequired();

            //modelBuilder.Entity<UserAccount>().Property(ua => ua.BirthDate).HasColumnType("date");
            //modelBuilder.Entity<UserAccount>().Property(ua => ua.BirthDate).IsRequired();
            //modelBuilder.Entity<UserAccount>().Property(ua => ua.FirstName).IsRequired();
            //modelBuilder.Entity<UserAccount>().Property(ua => ua.LastName).IsRequired();
            //modelBuilder.Entity<UserAccount>().Property(ua => ua.Email).IsRequired();

            //modelBuilder.Entity<Comment>().HasKey(c => c.ID);
            //modelBuilder.Entity<Comment>().Property(c => c.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //modelBuilder.Entity<Comment>().HasMany(c=>c.ChildComments).WithOptional(x=>x.ParentComment).WillCascadeOnDelete(false);
            //modelBuilder.Entity<Comment>().Property(c => c.Text).IsRequired();
            //modelBuilder.Entity<Comment>().Property(c => c.Time).IsRequired();

            base.OnModelCreating(modelBuilder);
        }

        private void InitializeDb()
        {
            Database.SetInitializer(new AuctionDbDropCreateInitializer());
            this.RegisterUserAccountChildTablesForDelete<UserAccount>();
        }
    }
}
