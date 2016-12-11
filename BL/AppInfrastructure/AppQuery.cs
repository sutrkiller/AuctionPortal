using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BL.AppInfrastructure
{
    public abstract class AppQuery<T> : EntityFrameworkQuery<T>
    {
        public new AuctionDbContext Context => (AuctionDbContext) base.Context;

        protected AppQuery(IUnitOfWorkProvider provider) : base(provider)
        {
            
        }
    }
}
