namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateTimeToDatetime2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Auctions", "AuctionStart", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Auctions", "AuctionEnd", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Bids", "BidTime", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Bids", "BidTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Auctions", "AuctionEnd", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Auctions", "AuctionStart", c => c.DateTime(nullable: false));
        }
    }
}
