using System.Data.Entity;
using DAL.Migrations;


namespace DAL.EF
{
    class AuctionDbMigrateInitializer : MigrateDatabaseToLatestVersion<AuctionDbContext, Configuration>
    {
        public AuctionDbMigrateInitializer()
        {
        }

        public AuctionDbMigrateInitializer(bool useSuppliedContext) : base(useSuppliedContext)
        {
        }

        public AuctionDbMigrateInitializer(bool useSuppliedContext, Configuration configuration) : base(useSuppliedContext, configuration)
        {
        }

        public AuctionDbMigrateInitializer(string connectionStringName) : base(connectionStringName)
        {
        }

        public override void InitializeDatabase(AuctionDbContext context)
        {
            base.InitializeDatabase(context);
        }
    }
}
