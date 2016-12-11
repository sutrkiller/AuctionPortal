using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BL.Repositories
{
    public class CommentRepository : EntityFrameworkRepository<Comment,long>
    {
        public CommentRepository(IUnitOfWorkProvider provider) : base(provider)
        {
        }
    }
}
