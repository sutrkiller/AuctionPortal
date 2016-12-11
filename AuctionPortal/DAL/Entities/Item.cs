using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Riganti.Utils.Infrastructure.Core;

namespace DAL.Entities
{
    public class Item : IEntity<long>
    {
        public long ID { get; set; }
        //public long AuctionId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }


        public Auction Auction { get; set; }
        public ICollection<ItemImage> ItemImages { get; set; } = new HashSet<ItemImage>();

    }
}
