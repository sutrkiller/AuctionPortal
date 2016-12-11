using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BL.AppInfrastructure
{
    class AppUnitOfWork : EntityFrameworkUnitOfWork
    {
        public new AuctionDbContext Context => (AuctionDbContext) base.Context;
        public AppUnitOfWork(IUnitOfWorkProvider provider, Func<DbContext> dbContextFactory, DbContextOptions options) : base(provider, dbContextFactory, options)
        {
        }
    }
}
