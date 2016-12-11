namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Auctions",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        AuctionStart = c.DateTime(nullable: false),
                        AuctionEnd = c.DateTime(nullable: false),
                        BasePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AuctionViews = c.Long(nullable: false),
                        DeliveryOptionsSerialized = c.String(),
                        PaymentOptionsSerialized = c.String(),
                        Category_ID = c.Long(nullable: false),
                        Seller_ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categories", t => t.Category_ID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.Seller_ID, cascadeDelete: true)
                .Index(t => t.Category_ID)
                .Index(t => t.Seller_ID);
            
            CreateTable(
                "dbo.Bids",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        BidTime = c.DateTime(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        User_ID = c.Long(nullable: false),
                        Auction_ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.User_ID, cascadeDelete: true)
                .ForeignKey("dbo.Auctions", t => t.Auction_ID)
                .Index(t => t.User_ID)
                .Index(t => t.Auction_ID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        UserAccount_Key = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserAccounts", t => t.UserAccount_Key, cascadeDelete: true)
                .Index(t => t.UserAccount_Key);
            
            CreateTable(
                "dbo.Deliveries",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        DeliveryType = c.Int(nullable: false),
                        PaymentMethod = c.Int(nullable: false),
                        DeliveryStatus = c.Int(nullable: false),
                        DeliveryAddress = c.String(nullable: false),
                        Buyer_ID = c.Long(nullable: false),
                        Seller_ID = c.Long(nullable: false),
                        Auction_ID = c.Long(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.Buyer_ID)
                .ForeignKey("dbo.Users", t => t.Seller_ID)
                .ForeignKey("dbo.Auctions", t => t.Auction_ID, cascadeDelete: true)
                .Index(t => t.Buyer_ID)
                .Index(t => t.Seller_ID)
                .Index(t => t.Auction_ID);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Text = c.String(),
                        Time = c.DateTime(nullable: false),
                        Author_ID = c.Long(nullable: false),
                        ParentComment_ID = c.Long(),
                        Auction_ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.Author_ID)
                .ForeignKey("dbo.Comments", t => t.ParentComment_ID)
                .ForeignKey("dbo.Auctions", t => t.Auction_ID, cascadeDelete: true)
                .Index(t => t.Author_ID)
                .Index(t => t.ParentComment_ID)
                .Index(t => t.Auction_ID);
            
            CreateTable(
                "dbo.UserAccounts",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Address = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        ID = c.Guid(nullable: false),
                        Tenant = c.String(nullable: false, maxLength: 50),
                        Username = c.String(nullable: false, maxLength: 254),
                        Created = c.DateTime(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                        IsAccountClosed = c.Boolean(nullable: false),
                        AccountClosed = c.DateTime(),
                        IsLoginAllowed = c.Boolean(nullable: false),
                        LastLogin = c.DateTime(),
                        LastFailedLogin = c.DateTime(),
                        FailedLoginCount = c.Int(nullable: false),
                        PasswordChanged = c.DateTime(),
                        RequiresPasswordReset = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 254),
                        IsAccountVerified = c.Boolean(nullable: false),
                        LastFailedPasswordReset = c.DateTime(),
                        FailedPasswordResetCount = c.Int(nullable: false),
                        MobileCode = c.String(maxLength: 100),
                        MobileCodeSent = c.DateTime(),
                        MobilePhoneNumber = c.String(maxLength: 20),
                        MobilePhoneNumberChanged = c.DateTime(),
                        AccountTwoFactorAuthMode = c.Int(nullable: false),
                        CurrentTwoFactorAuthStatus = c.Int(nullable: false),
                        VerificationKey = c.String(maxLength: 100),
                        VerificationPurpose = c.Int(),
                        VerificationKeySent = c.DateTime(),
                        VerificationStorage = c.String(maxLength: 100),
                        HashedPassword = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        ParentKey = c.Int(nullable: false),
                        Type = c.String(nullable: false, maxLength: 150),
                        Value = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.UserAccounts", t => t.ParentKey, cascadeDelete: true)
                .Index(t => t.ParentKey);
            
            CreateTable(
                "dbo.LinkedAccountClaims",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        ParentKey = c.Int(nullable: false),
                        ProviderName = c.String(nullable: false, maxLength: 30),
                        ProviderAccountID = c.String(nullable: false, maxLength: 100),
                        Type = c.String(nullable: false, maxLength: 150),
                        Value = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.UserAccounts", t => t.ParentKey, cascadeDelete: true)
                .Index(t => t.ParentKey);
            
            CreateTable(
                "dbo.LinkedAccounts",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        ParentKey = c.Int(nullable: false),
                        ProviderName = c.String(nullable: false, maxLength: 30),
                        ProviderAccountID = c.String(nullable: false, maxLength: 100),
                        LastLogin = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.UserAccounts", t => t.ParentKey, cascadeDelete: true)
                .Index(t => t.ParentKey);
            
            CreateTable(
                "dbo.PasswordResetSecrets",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        ParentKey = c.Int(nullable: false),
                        PasswordResetSecretID = c.Guid(nullable: false),
                        Question = c.String(nullable: false, maxLength: 150),
                        Answer = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.UserAccounts", t => t.ParentKey, cascadeDelete: true)
                .Index(t => t.ParentKey);
            
            CreateTable(
                "dbo.TwoFactorAuthTokens",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        ParentKey = c.Int(nullable: false),
                        Token = c.String(nullable: false, maxLength: 100),
                        Issued = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.UserAccounts", t => t.ParentKey, cascadeDelete: true)
                .Index(t => t.ParentKey);
            
            CreateTable(
                "dbo.UserCertificates",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        ParentKey = c.Int(nullable: false),
                        Thumbprint = c.String(nullable: false, maxLength: 150),
                        Subject = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.UserAccounts", t => t.ParentKey, cascadeDelete: true)
                .Index(t => t.ParentKey);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Auction_ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Auctions", t => t.Auction_ID, cascadeDelete: true)
                .Index(t => t.Auction_ID);
            
            CreateTable(
                "dbo.ItemImages",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Image = c.Binary(nullable: false),
                        Item_ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Items", t => t.Item_ID, cascadeDelete: true)
                .Index(t => t.Item_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Auctions", "Seller_ID", "dbo.Users");
            DropForeignKey("dbo.Items", "Auction_ID", "dbo.Auctions");
            DropForeignKey("dbo.ItemImages", "Item_ID", "dbo.Items");
            DropForeignKey("dbo.Deliveries", "Auction_ID", "dbo.Auctions");
            DropForeignKey("dbo.Comments", "Auction_ID", "dbo.Auctions");
            DropForeignKey("dbo.Auctions", "Category_ID", "dbo.Categories");
            DropForeignKey("dbo.Bids", "Auction_ID", "dbo.Auctions");
            DropForeignKey("dbo.Bids", "User_ID", "dbo.Users");
            DropForeignKey("dbo.Users", "UserAccount_Key", "dbo.UserAccounts");
            DropForeignKey("dbo.UserCertificates", "ParentKey", "dbo.UserAccounts");
            DropForeignKey("dbo.TwoFactorAuthTokens", "ParentKey", "dbo.UserAccounts");
            DropForeignKey("dbo.PasswordResetSecrets", "ParentKey", "dbo.UserAccounts");
            DropForeignKey("dbo.LinkedAccounts", "ParentKey", "dbo.UserAccounts");
            DropForeignKey("dbo.LinkedAccountClaims", "ParentKey", "dbo.UserAccounts");
            DropForeignKey("dbo.UserClaims", "ParentKey", "dbo.UserAccounts");
            DropForeignKey("dbo.Comments", "ParentComment_ID", "dbo.Comments");
            DropForeignKey("dbo.Comments", "Author_ID", "dbo.Users");
            DropForeignKey("dbo.Deliveries", "Seller_ID", "dbo.Users");
            DropForeignKey("dbo.Deliveries", "Buyer_ID", "dbo.Users");
            DropIndex("dbo.ItemImages", new[] { "Item_ID" });
            DropIndex("dbo.Items", new[] { "Auction_ID" });
            DropIndex("dbo.Categories", new[] { "Name" });
            DropIndex("dbo.UserCertificates", new[] { "ParentKey" });
            DropIndex("dbo.TwoFactorAuthTokens", new[] { "ParentKey" });
            DropIndex("dbo.PasswordResetSecrets", new[] { "ParentKey" });
            DropIndex("dbo.LinkedAccounts", new[] { "ParentKey" });
            DropIndex("dbo.LinkedAccountClaims", new[] { "ParentKey" });
            DropIndex("dbo.UserClaims", new[] { "ParentKey" });
            DropIndex("dbo.Comments", new[] { "Auction_ID" });
            DropIndex("dbo.Comments", new[] { "ParentComment_ID" });
            DropIndex("dbo.Comments", new[] { "Author_ID" });
            DropIndex("dbo.Deliveries", new[] { "Auction_ID" });
            DropIndex("dbo.Deliveries", new[] { "Seller_ID" });
            DropIndex("dbo.Deliveries", new[] { "Buyer_ID" });
            DropIndex("dbo.Users", new[] { "UserAccount_Key" });
            DropIndex("dbo.Bids", new[] { "Auction_ID" });
            DropIndex("dbo.Bids", new[] { "User_ID" });
            DropIndex("dbo.Auctions", new[] { "Seller_ID" });
            DropIndex("dbo.Auctions", new[] { "Category_ID" });
            DropTable("dbo.ItemImages");
            DropTable("dbo.Items");
            DropTable("dbo.Categories");
            DropTable("dbo.UserCertificates");
            DropTable("dbo.TwoFactorAuthTokens");
            DropTable("dbo.PasswordResetSecrets");
            DropTable("dbo.LinkedAccounts");
            DropTable("dbo.LinkedAccountClaims");
            DropTable("dbo.UserClaims");
            DropTable("dbo.UserAccounts");
            DropTable("dbo.Comments");
            DropTable("dbo.Deliveries");
            DropTable("dbo.Users");
            DropTable("dbo.Bids");
            DropTable("dbo.Auctions");
        }
    }
}
