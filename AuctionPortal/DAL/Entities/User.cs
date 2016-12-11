using System.Collections.Generic;
using Autentization;
using Riganti.Utils.Infrastructure.Core;

namespace DAL.Entities
{
    public class User : IEntity<long>
    {
        public long ID { get; set; }
        //public long AddressId { get; set; }
        //public long UserAccountId { get; set; }

        public virtual UserAccount UserAccount { get; set; }
        //public virtual Address Address { get; set; }
        public virtual ICollection<Auction> Auctions { get; set; } = new HashSet<Auction>();
        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public virtual ICollection<Bid> Bids { get; set; } = new HashSet<Bid>();
        public virtual ICollection<Delivery> SoldItemsDeliveries { get; set; } = new HashSet<Delivery>();
        public virtual ICollection<Delivery> BoughtItemsDeliveries { get; set; } = new HashSet<Delivery>();
        
        
    }
}
