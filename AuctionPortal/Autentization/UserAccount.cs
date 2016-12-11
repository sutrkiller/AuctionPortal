using System;
using System.ComponentModel.DataAnnotations;
using BrockAllen.MembershipReboot.Relational;
using Riganti.Utils.Infrastructure.Core;

namespace Autentization
{
    public class UserAccount : RelationalUserAccount, IEntity<Guid>
    {
        
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Address { get; set; }
        [DataType(DataType.Date)]
        public virtual DateTime BirthDate { get; set; } = new DateTime(1960,1,1);

    }
}
