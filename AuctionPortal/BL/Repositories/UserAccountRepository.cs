using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autentization;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BL.Repositories
{
    public class UserAccountRepository : EntityFrameworkRepository<UserAccount,Guid>
    {
        public UserAccountRepository(IUnitOfWorkProvider provider) : base(provider)
        {
        }
    }
}
