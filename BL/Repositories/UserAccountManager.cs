using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Ef;
using DAL.EF;
using UserAccount = Autentization.UserAccount;

namespace BL.Repositories
{
    public class UserAccountManager : DbContextUserAccountRepository<AuctionDbContext,UserAccount>, IUserAccountRepository<UserAccount>
    {
        public UserAccountManager(Func<DbContext> dbContextFactory)
            : base(dbContextFactory.Invoke() as AuctionDbContext)
        {        
        }

        protected override IQueryable<UserAccount> DefaultQueryFilter(IQueryable<UserAccount> query, string filter)
        {
            if (query == null) { throw new ArgumentNullException(nameof(query));}
            if (filter == null) { throw new ArgumentNullException(nameof(filter));}

            return query.SelectMany(ac => ac.ClaimCollection, (acc, claims) => new {acc, claims})
                .Where(t => t.claims.Value.Contains(filter))
                .Select(t => t.acc);
        }
    }
}
