namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BL_changes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserAccounts", "AccountApproved", c => c.DateTime());
            AddColumn("dbo.UserAccounts", "AccountRejected", c => c.DateTime());
            AddColumn("dbo.ItemImages", "ImagePath", c => c.String());
            AlterColumn("dbo.LinkedAccountClaims", "ProviderName", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.LinkedAccounts", "ProviderName", c => c.String(nullable: false, maxLength: 200));
            DropColumn("dbo.ItemImages", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ItemImages", "Image", c => c.Binary(nullable: false));
            AlterColumn("dbo.LinkedAccounts", "ProviderName", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.LinkedAccountClaims", "ProviderName", c => c.String(nullable: false, maxLength: 30));
            DropColumn("dbo.ItemImages", "ImagePath");
            DropColumn("dbo.UserAccounts", "AccountRejected");
            DropColumn("dbo.UserAccounts", "AccountApproved");
        }
    }
}
