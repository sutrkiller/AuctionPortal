using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riganti.Utils.Infrastructure.Core;

namespace BL.Services
{
    public abstract class BaseService
    {
        public IUnitOfWorkProvider UnitOfWorkProvider { get; set; }
    }
}
